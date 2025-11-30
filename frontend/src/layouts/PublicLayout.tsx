import '@/pages/public/PublicHeader.css'
import { Outlet, useLocation } from "react-router-dom";
import PublicHeader from "@/pages/public/PublicHeader";

export default function PublicLayout() {
    const location = useLocation();
    const isHome = location.pathname === "/";

    return (
        <div className="flex flex-col min-h-screen">
            <PublicHeader />
            <main className={
                isHome
                    ? ""         
                    : "flex flex-1"    
            }>
                <Outlet />
            </main>
        </div>
    );
}