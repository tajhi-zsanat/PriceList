import { createContext, useContext, useMemo, useState, useEffect, type ReactNode } from "react";
import { loginApi, logoutApi, /*type AuthUser,*/ type LoginDto } from "./auth.api";
import { clearAccessToken, decodeJwt, getAccessToken, setAccessToken } from "./token";

export type Role = "Admin" | "Editor" | "User";
// type AuthUserOrNull = AuthUser | null;

type AuthCtx = {
  // user: AuthUserOrNull;
  accessToken: string | null;
  login: (dto: LoginDto) => Promise<void>;
  logout: () => Promise<void>;
  hasRole: (r: Role | Role[]) => boolean;
  isAuthenticated: boolean;
  authReady: boolean; // ⬅️ new
};

const Ctx = createContext<AuthCtx | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
  // Hydrate synchronously on the first render
  const [accessToken, setTokenState] = useState<string | null>(() => getAccessToken());
  // const [user, setUser] = useState<AuthUserOrNull>(null);

  // authReady becomes true after the very first effect tick. This gives time
  // for any browser specifics, and is a good place to kick a /me call if you add one.
  const [authReady, setAuthReady] = useState(false);
  useEffect(() => {
    setAuthReady(true);
  }, []);

  const login = async (dto: LoginDto) => {
    const res = await loginApi(dto);
    // persist token + user
    setAccessToken(res.accessToken);
    setTokenState(res.accessToken);
    // setUser(res.user);
    // localStorage.setItem(LS_USER_KEY, JSON.stringify(res.user));
  };

  const logout = async () => {
    await logoutApi().catch(() => { });
    clearAccessToken();
    setTokenState(null);
    // setUser(null);
    // localStorage.removeItem(LS_USER_KEY);
  };

  const hasRole = (r: Role | Role[]) => {
    const token = getAccessToken();
    if (!accessToken) return false;

    const payload = decodeJwt<any>(token);
    if (!payload) return false;

    const roleClaim =
      "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";

    const claimValue = payload[roleClaim];
    const roles: string[] = claimValue
      ? Array.isArray(claimValue)
        ? claimValue
        : [claimValue]
      : [];

    const needed = Array.isArray(r) ? r : [r];
    return needed.some((role) => roles.includes(role));
  };
  
  // If you want “token-only” pages to work even before user is known,
  // you can loosen this to: const isAuthenticated = !!accessToken;
  const isAuthenticated = !!accessToken;

  const value = useMemo<AuthCtx>(() => ({
    // user,
    accessToken,
    login,
    logout,
    hasRole,
    isAuthenticated,
    authReady,
  }), [/*user,*/ accessToken, authReady]);

  return <Ctx.Provider value={value}>{children}</Ctx.Provider>;
}

export function useAuth() {
  const ctx = useContext(Ctx);
  if (!ctx) throw new Error("useAuth must be used inside <AuthProvider>");
  return ctx;
}
