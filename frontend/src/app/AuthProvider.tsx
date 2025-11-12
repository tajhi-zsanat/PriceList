import { createContext, useContext, useMemo, useState, useEffect, type ReactNode } from "react";
import { loginApi, logoutApi, type AuthUser, type LoginDto } from "./auth.api";
import { clearAccessToken, decodeJwt, getAccessToken, setAccessToken } from "./token";

export type Role = "Admin" | "Editor" | "User";
type AuthUserOrNull = AuthUser | null;

type AuthCtx = {
  user: AuthUserOrNull;
  accessToken: string | null;
  login: (dto: LoginDto) => Promise<void>;
  logout: () => Promise<void>;
  hasRole: (r: Role | Role[]) => boolean;
  isAuthenticated: boolean;
  authReady: boolean; // ⬅️ new
};

const Ctx = createContext<AuthCtx | null>(null);

const LS_USER_KEY = "pl_user";

function loadUserFromStorage(): AuthUserOrNull {
  try {
    const raw = localStorage.getItem(LS_USER_KEY);
    return raw ? (JSON.parse(raw) as AuthUser) : null;
  } catch {
    return null;
  }
}

export function AuthProvider({ children }: { children: ReactNode }) {
  // Hydrate synchronously on the first render
  const [accessToken, setTokenState] = useState<string | null>(() => getAccessToken());
  const [user, setUser] = useState<AuthUserOrNull>(() => {
    // 1) Prefer persisted user
    const u = loadUserFromStorage();
    if (u) return u;

    // 2) Fallback: try infer minimal user from token (only if your token has claims)
    const token = getAccessToken();
    if (token) {
      const payload = decodeJwt<any>(token);
      if (payload?.name && payload?.roles) {
        return {
          id: Number(payload.sub) || 0,
          name: payload.name,
          roles: payload.roles as Role[],
        };
      }
    }
    return null;
  });

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
    setUser(res.user);
    localStorage.setItem(LS_USER_KEY, JSON.stringify(res.user));
  };

  const logout = async () => {
    await logoutApi().catch(() => {});
    clearAccessToken();
    setTokenState(null);
    setUser(null);
    localStorage.removeItem(LS_USER_KEY);
  };

  const hasRole = (r: Role | Role[]) => {
    if (!user) return false;
    const needed = Array.isArray(r) ? r : [r];
    return needed.some(role => user.roles.includes(role));
  };

  // If you want “token-only” pages to work even before user is known,
  // you can loosen this to: const isAuthenticated = !!accessToken;
  const isAuthenticated = !!user && !!accessToken;

  const value = useMemo<AuthCtx>(() => ({
    user,
    accessToken,
    login,
    logout,
    hasRole,
    isAuthenticated,
    authReady,
  }), [user, accessToken, authReady]);

  return <Ctx.Provider value={value}>{children}</Ctx.Provider>;
}

export function useAuth() {
  const ctx = useContext(Ctx);
  if (!ctx) throw new Error("useAuth must be used inside <AuthProvider>");
  return ctx;
}
