// src/app/AuthProvider.tsx
import { createContext, useContext, useMemo, useState, type ReactNode} from "react";

type Role = "Admin" | "Editor" | "User";
type AuthUser = { id: number; name: string; roles: Role[] } | null;

type AuthCtx = {
  user: AuthUser;
  login: (u: AuthUser) => void;
  logout: () => void;
  hasRole: (r: Role | Role[]) => boolean;
};

const Ctx = createContext<AuthCtx | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<AuthUser>(null);

  const value = useMemo<AuthCtx>(() => ({
    user,
    login: setUser,
    logout: () => setUser(null),
    hasRole: (r) => {
      if (!user) return false;
      const need = Array.isArray(r) ? r : [r];
      return need.some(role => user.roles.includes(role));
    }
  }), [user]);

  return <Ctx.Provider value={value}>{children}</Ctx.Provider>;
}

export function useAuth() {
  const ctx = useContext(Ctx);
  if (!ctx) throw new Error("useAuth must be used within AuthProvider");
  return ctx;
}
