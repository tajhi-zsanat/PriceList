import heroImage from "@/assets/img/home/hero-Main.webp";
import whatsappIcon from "@/assets/img/home/logos_whatsapp-icon.png";
import shopIcon from "@/assets/img/home/shop.png";
import FarsiText from "../FarsiText";

export default function Hero() {
    return (
        <div className="relative my-8 max-w-[1920px] mx-auto" dir="rtl">
            <img className="w-full" src={heroImage} alt="بنر اصلی" loading="lazy" />

            <div
                className="
                    absolute top-1/2 right-1/2 
                    translate-x-1/2 -translate-y-1/2
                    flex flex-col items-center justify-center gap-3
                    text-center max-w-[90%]
                "
            >
                <p className="text-white text-[24px] font-semibold drop-shadow">
                    چطور از پیکاتک خرید کنیم؟
                </p>

                <p className="text-[18px] text-white font-medium drop-shadow">
                    برای خرید و مشاوره کافیست با ما تماس بگیرید.
                </p>

                <p className="text-[18px] text-white font-medium drop-shadow">
                    همچنین می‌توانید از طریق وبسایت پیکاتک، کالای مورد نظر خود را به‌صورت آنلاین سفارش دهید.
                </p>

                <p className="text-[18px] text-white font-medium mt-4 drop-shadow">
                    برای خرید همین حالا تماس بگیرید.
                </p>

                <div className="flex justify-center items-center gap-3 mt-4 flex-wrap">
                    <a
                        href="https://wa.me/989374371848"
                        target="_blank"
                        rel="noopener noreferrer"
                        className="
    flex items-center gap-2 text-white bg-[#1F78AE]
    rounded-[8px] px-6 py-2 text-[16px] font-medium
    hover:bg-[#166088] transition cursor-pointer
  "
                    >
                        <img src={whatsappIcon} alt="" className="w-6 h-6" />
                        <FarsiText>98-9374371848+</FarsiText>
                    </a>


                    <a href="https://pikatak.com/" target="_blank"
                        className="
                            flex items-center gap-2 text-[#1B1B1E] bg-white
                            rounded-[8px] px-6 py-2 text-[16px] font-medium
                            hover:bg-gray-100 transition cursor-pointer
                        "
                    >
                        <img src={shopIcon} alt="" className="w-6 h-6" />
                        خرید از وبسایت پیکاتک
                    </a>
                </div>
            </div>
        </div>
    );
}
