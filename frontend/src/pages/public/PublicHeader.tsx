import { useEffect, useRef, useState } from "react";
import { Link, NavLink, useLocation, matchPath } from "react-router-dom";

import logo from "@/assets/img/pikatak.png";
import user from "@/assets/img/user.png";
import back from "@/assets/img/back-icon-mobile.png";

export default function PublicHeader() {
  const location = useLocation();
  const [hide, setHide] = useState(false);
  const lastScrollY = useRef(0);

  const isCategories =
    !!(
      matchPath("/category", location.pathname) ||
      matchPath("/category/:categoryId/groups", location.pathname) ||
      matchPath(
        "/category/:categoryId/groups/:groupId/types",
        location.pathname
      ) ||
      matchPath(
        "/category/:categoryId/groups/:groupId/brands",
        location.pathname
      ) ||
      matchPath(
        "/category/:categoryId/groups/:groupId/brands/:brandId/products",
        location.pathname
      )
    );

  useEffect(() => {
    const handleScroll = () => {
      const current = window.scrollY;

      if (current > lastScrollY.current && current > 80) {
        setHide(true);
      } else {
        setHide(false);
      }

      lastScrollY.current = current;
    };

    window.addEventListener("scroll", handleScroll);
    return () => window.removeEventListener("scroll", handleScroll);
  }, []);

  const navItems: { to: string; label: string; isActive?: boolean }[] = [
    { to: "/", label: "خانه" },
    { to: "/category", label: "دسته‌بندی‌ها", isActive: isCategories },
    { to: "/support", label: "پشتیبانی" },
    { to: "/rate", label: "تعرفه‌ها" },
  ];

  return (
    <header
      className={`
        sticky top-0 z-50 bg-white shadow-md 
        transition-transform duration-300
        ${hide ? "-translate-y-full" : "translate-y-0"}
      `}
    >
      <nav dir="rtl">
        {/* Mobile back button */}
        <img className="block sm:hidden" src={back} alt="back" />

        {/* Desktop top bar */}
        <div className="hidden sm:flex justify-between items-center px-16 pt-10 pb-4">
          <img src={logo} alt="Logo" />
          <Link to="/login" className="header-login p-1.5">
            <img src={user} alt="login" className="header-logn-img" />
            <span>ورود | ثبت نام</span>
          </Link>
        </div>

        {/* Desktop navigation */}
        <div className="hidden sm:flex gap-10 px-16 pb-3">
          {navItems.map((item) => (
            <NavLink
              key={item.to}
              to={item.to}
              end
              className={({ isActive }) =>
                `cursor-pointer text-base font-medium transition
                ${
                  item.isActive || isActive
                    ? "text-[#1F78AE]"
                    : "text-[#3F414D] hover:text-[#1F78AE]"
                }`
              }
            >
              {item.label}
            </NavLink>
          ))}
        </div>
      </nav>
    </header>
  );
}
