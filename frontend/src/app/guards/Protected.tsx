import { Navigate, Outlet, useLocation } from "react-router-dom";
import { useAuth } from "../AuthProvider";

export default function Protected() {
  const { user } = useAuth();
  const loc = useLocation();
  return user ? <Outlet /> : <Navigate to="/login" replace state={{ from: loc }} />;
  //return <Outlet />
}