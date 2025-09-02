import logo from "./assets/img/pikatak.png";
import user from "./assets/img/user.png";
import back from "./assets/img/back-icon-mobile.png";
import './Header.css'
import { Link } from "react-router-dom";

export default function Header() {
    return (
        <header className="">
            <nav className="">
                <img className="block sm:hidden" src={back} alt="back" />
                <div className="hidden sm:flex justify-between items-center px-16 pt-10 pb-4">
                    <img src={logo} alt="Logo" />
                    <button className="header-login p-1.5">
                        <img src={user} alt="login" className="header-logn-img" />
                        <span>ورود | ثبت نام</span>
                    </button>
                </div>
                <div className="hidden sm:flex gap-10 container-navabr px-16 py-3">
                    <a className="text-[#636363]" href="https://pikatak.com/">دسته‌بندی کالاها</a>
                    <a className="text-[#636363]" href="https://pikatak.com/manufacturer/all">تولید کنندگان</a>
                    <Link to="/" className="text-[#1F78AE]">لیست قیمت محصولات</Link>
                </div>
            </nav>
        </header>
    );
}