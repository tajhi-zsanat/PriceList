import AdminHeader from "@/pages/admin/AdminHeader";
import { Outlet } from "react-router-dom";
import { Toaster } from "@/components/ui/sonner";
import useForms from "@/hook/useForms";

export default function AdminLayout() {
  const formsHook = useForms();

  return (
    <div className="flex flex-col min-h-screen p-4">
      <AdminHeader onFormCreated={formsHook.reload} />
      <main className="flex flex-1 ">
        <Outlet context={formsHook} />
        <Toaster richColors position="top-right" />
      </main>
    </div>
  );
};