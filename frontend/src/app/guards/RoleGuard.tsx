import { Outlet, Navigate } from "react-router-dom";
import { useAuth } from "../AuthProvider";

export default function RoleGuard({ roles }: { roles: Array<"Admin" | "Editor" | "User"> }) {
  const { hasRole } = useAuth();
  return hasRole(roles) ? <Outlet /> : <Navigate to="/403" replace />;
}