import '@/pages/public/PublicHeader.css'
import { Outlet, useLocation } from "react-router-dom";
import PublicHeader from "@/pages/public/PublicHeader";
import Footer from '@/pages/public/Footer';
import ScrollToTop from '@/components/ScrollToTop';

export default function PublicLayout() {
    const location = useLocation();
    const isHome = location.pathname === "/";

    return (
        <>
            <ScrollToTop />
            <div className="flex flex-col min-h-screen">
                <PublicHeader />
                <main className={
                    isHome
                        ? ""
                        : "flex flex-1"
                }>
                    <Outlet />
                </main>
                <Footer />
            </div>
        </>
    );
}