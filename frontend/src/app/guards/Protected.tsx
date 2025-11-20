import { Navigate, Outlet, useLocation } from "react-router-dom";
import { useAuth } from "@/app/AuthProvider";

export default function Protected() {
  const { isAuthenticated, authReady } = useAuth();
  const loc = useLocation();
  // Wait for hydration to finish; avoids false redirect on first render.
  if (!authReady) return null;
 
  if (!isAuthenticated) {
    return <Navigate to="/login" replace state={{ from: loc.pathname + loc.search }} />;
  }
  return <Outlet />;
}
