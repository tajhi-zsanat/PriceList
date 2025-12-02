import { FaYoutube, FaInstagram, FaLinkedin, FaTelegram } from "react-icons/fa";
import { SiAparat } from "react-icons/si";

import arrowIcon from "@/assets/img/footer/arrow-icon.png";
import eNamad from "@/assets/img/footer/eNamad.png";

export default function Footer() {

    return (
        <footer className="bg-[#ECEFF1] pb-12">
            <div className="flex flex-col">
                <div className="bg-[#F5F5F5] pt-16 pb-8">
                    <div
                        className="flex justify-center items-center w-16 h-16 bg-[#d3d3d3] rounded-full cursor-pointer mx-auto mt-2"
                        onClick={() => window.scrollTo({ top: 0, behavior: "smooth" })}
                    >
                        <img src={arrowIcon} className="w-6 h-[25px]" alt="" loading="lazy" />
                    </div>
                </div>

                <div className="bg-[#3f414d] text-base font-medium">
                    <p className="text-white py-4 text-center">
                        <span>تلفن پشتیبانی: </span>
                        <a href="tel:+982187700142" className="text-[#0d6efd]">
                            87700142-021 ( 30 خط ویژه )
                        </a>
                        <span> | همه روزه پاسخگوی شما هستیم. </span>
                    </p>
                </div>
            </div>

            <div className="bg-[#ECEFF1] text-[#666666] py-4 px-8 max-w-6xl mx-auto">
                <div className="flex items-center justify-between">
                    <div className="flex flex-col">
                        <span>پیکاتک رشد و توسعه کسب و کار صنعتی</span>
                        <span>پیکاتک با شعار رشد و توسعه کسب و کارهای صنعتی یک سامانه تخصصی خرید و فروش آنلاین محصولات صنعتی می باشد</span>
                    </div>

                    <a className="bg-white rounded-2xl max-w-20 p-2" href="https://trustseal.enamad.ir/?id=284897&Code=2GULLqA9yIwU4WqZJLCw">
                        <img src={eNamad} alt="" />
                    </a>
                </div>
            </div>

            <div className="grid grid-cols-2 md:grid-cols-4 py-4 px-8 max-w-6xl mx-auto">
                <div className="flex flex-col gap-4">
                    <p className="text-base font-medium">اطلاعات</p>
                    <a className="text-[#666666] hover:text-black transition-all" target="_blank" href="https://pikatak.com/panel/%d8%aa%d8%b9%d8%b1%d9%81%d9%87-%d8%ae%d8%af%d9%85%d8%a7%d8%aa-%d9%be%db%8c%da%a9%d8%a7%d8%aa%da%a9/">تعرفه خدمات پیکاتک</a>
                    <a className="text-[#666666] hover:text-black transition-all" target="_blank" href="https://pikatak.com/panel/%d8%ac%d8%af%d9%88%d9%84-%d9%85%d8%ad%d8%a7%d8%b3%d8%a8%d9%87-%da%a9%d8%a7%d8%b1%d9%85%d8%b2%d8%af-%d9%81%d8%b1%d9%88%d8%b4/">همکاری در فروش پیکاتک</a>
                    <a className="text-[#666666] hover:text-black transition-all" target="_blank" href="https://pikatak.com/%D9%87%D9%85%DA%A9%D8%A7%D8%B1%DB%8C-%D8%AF%D8%B1-%D9%81%D8%B1%D9%88%D8%B4-%D9%BE%DB%8C%DA%A9%D8%A7%D8%AA%DA%A9">قرار داد همکاری در فروش</a>
                    <a className="text-[#666666] hover:text-black transition-all" target="_blank" href="https://pikatak.com/shipping-returns">رویه های بازگرداندن کالا</a>
                    <a className="text-[#666666] hover:text-black transition-all" target="_blank" href="https://pikatak.com/privacy-notice">حریم خصوصی</a>
                    <a className="text-[#666666] hover:text-black transition-all" target="_blank" href="https://pikatak.com/conditions-of-use">شرایط استفاده</a>
                    <a className="text-[#666666] hover:text-black transition-all" target="_blank" href="https://pikatak.com/about-us">درباره پیکاتک</a>
                </div>
                <div className="flex flex-col gap-4">
                    <p className="text-base font-medium">خدمات مشتریان</p>
                    <a className="text-[#666666] hover:text-black transition-all" target="_blank" href="https://pikatak.com/contactus">تماس با ما</a>
                    <a className="text-[#666666] hover:text-black transition-all" target="_blank" href="https://pikatak.com/blog">بلاگ</a>
                    <a className="text-[#666666] hover:text-black transition-all" target="_blank" href="https://pikatak.com/mag/">پیکامگ</a>
                    <a className="text-[#666666] hover:text-black transition-all" target="_blank" href="https://pikatak.com/%D9%81%D8%B1%D9%88%D8%B4-%D8%A7%D9%82%D8%B3%D8%A7%D8%B7%DB%8C-%DA%A9%D8%A7%D9%84%D8%A7-%D8%AF%D8%B1-%D9%BE%DB%8C%DA%A9%D8%A7%D8%AA%DA%A9">فروش اقساطی کالا در پیکاتک</a>
                    <a className="text-[#666666] hover:text-black transition-all" target="_blank" href="https://pikatak.com/%D8%AF%D8%B1%DB%8C%D8%A7%D9%81%D8%AA-%D8%A7%D8%B9%D8%AA%D8%A8%D8%A7%D8%B1-%D8%A7%D8%B2-%D9%BE%DB%8C%DA%A9%D8%A7%D8%AA%DA%A9">دریافت اعتبار از پیکاتک</a>
                    <a className="text-[#666666] hover:text-black transition-all" target="_blank" href="https://pikatak.com/panel/category/%d8%a2%d9%85%d9%88%d8%b2%d8%b4/%d8%a2%d9%85%d9%88%d8%b2%d8%b4-%d9%81%d8%b1%d9%88%d8%b4%d9%86%d8%af%da%af%d8%a7%d9%86/">آموزش فروشندگان</a>
                    <a className="text-[#666666] hover:text-black transition-all" target="_blank" href="https://pikatak.com/panel/category/%d8%a2%d9%85%d9%88%d8%b2%d8%b4/%d8%a2%d9%85%d9%88%d8%b2%d8%b4-%d8%ae%d8%b1%db%8c%d8%af%d8%a7%d8%b1%d8%a7%d9%86/">آموزش خریداران</a>
                </div>
                <div className="flex flex-col gap-4">
                    <p className="text-base font-medium">حساب من</p>
                    <a className="text-[#666666] hover:text-black transition-all" target="_blank" href="https://pikatak.com/login?ReturnUrl=%2Fcustomer%2Finfo">حساب من</a>
                    <a className="text-[#666666] hover:text-black transition-all" target="_blank" href="https://pikatak.com/login/checkoutasguest?returnUrl=%2Fonepagecheckout">سبد خرید</a>
                    <a className="text-[#666666] hover:text-black transition-all" target="_blank" href="https://pikatak.com/login?ReturnUrl=%2FBookmark%2FList">علاقه مندی ها</a>
                    <a className="text-[#666666] hover:text-black transition-all" target="_blank" href="https://pikatak.com/compareproducts">فهرست مقایسه محصولات</a>
                    <a className="text-[#666666] hover:text-black transition-all" target="_blank" href="https://pikatak.com/privacy-notice">حریم خصوصی</a>
                    <a className="text-[#666666] hover:text-black transition-all" target="_blank" href="https://pikatak.com/Supplier">ثبت نام تامین کنندگان</a>
                </div>
                <div className="flex flex-col gap-3 text-[#3F414D]" dir="rtl">
                    <p className="font-medium text-base">شبکه‌های اجتماعی</p>

                    <div className="flex items-center gap-4 text-[#3F414D]">
                        <a
                            href="https://www.youtube.com/"
                            target="_blank"
                            rel="noopener noreferrer"
                            className="hover:text-red-600 transition text-2xl"
                        >
                            <FaYoutube />
                        </a>

                        <a
                            href="https://www.instagram.com/pikatak_com/#"
                            target="_blank"
                            rel="noopener noreferrer"
                            className="hover:text-pink-600 transition text-2xl"
                        >
                            <FaInstagram />
                        </a>

                        <a
                            href="https://www.aparat.com/pikatak_com"
                            target="_blank"
                            rel="noopener noreferrer"
                            className="hover:text-[#ED145B] transition text-2xl"
                        >
                            <SiAparat />
                        </a>

                        <a
                            href="https://www.linkedin.com/in/pikatakcom/"
                            target="_blank"
                            rel="noopener noreferrer"
                            className="hover:text-blue-700 transition text-2xl"
                        >
                            <FaLinkedin />
                        </a>

                        <a
                            href="https://t.me/pikatak_com"
                            target="_blank"
                            rel="noopener noreferrer"
                            className="hover:text-blue-500 transition text-2xl"
                        >
                            <FaTelegram />
                        </a>
                    </div>
                </div>

            </div>
            <div className="w-full h-px bg-[#CFD8DC]"></div>
        </footer>
    );
}
