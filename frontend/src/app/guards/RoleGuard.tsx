// src/app/guards/RoleGuard.tsx
import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "@/app/AuthProvider";
import type { Role } from "@/app/auth.api";

export default function RoleGuard({ roles }: { roles: Role[] }) {
  const { hasRole } = useAuth();
  const ok = roles.some(r => hasRole(r));
  if (!ok) return <Navigate to="/403" replace />;
  return <Outlet />;
}
