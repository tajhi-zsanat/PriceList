import AdminHeader from "@/pages/admin/AdminHeader";
import { Outlet } from "react-router-dom";

export default function AdminLayout() {
  return (
    <div className="flex flex-col min-h-screen p-4">
      <AdminHeader />
      <main className="flex flex-1 ">
        <Outlet />
      </main>
    </div>
  );
};