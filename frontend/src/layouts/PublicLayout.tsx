import '@/pages/public/PublicHeader.css'
import { Outlet } from "react-router-dom";
import PublicHeader from "@/pages/public/PublicHeader";

export default function PublicLayout() {
    return (
        <div className="flex flex-col min-h-screen">
            <PublicHeader />
            <main className="flex flex-1 ">
                <Outlet />
            </main>
        </div>
    );
}