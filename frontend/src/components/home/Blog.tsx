import blog1 from "@/assets/img/home/blog1.png";
import arrowLeft from "@/assets/img/home/arrrow-left-link.png";

export default function Blog() {
    return (
        <div className="max-w-6xl m-auto my-8">
            <div className="flex items-center justify-between">
                <p className="text-[26px] font-medium text-[#1B1B1E]">بلاگ</p>
                <a href="https://pikatak.com/blog" target="_blank" 
                className="flex items-center gap-1">
                    <span>مشاهده همه</span>
                    <img className="" src={arrowLeft} alt="" loading="lazy" />
                </a>
            </div>
            <ul className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4 mt-4">
                <a
                    href="https://pikatak.com/%DB%B1%DB%B0-%DA%A9%D8%A7%D8%B1%D8%A8%D8%B1%D8%AF-%D9%85%D9%87%D9%85-%DA%AF%DB%8C%D8%AC-%D9%81%D8%B4%D8%A7%D8%B1-%D8%B1%D9%88%D8%BA%D9%86%DB%8C-%D8%AF%D8%B1-%D8%B5%D9%86%D8%B9%D8%AA"
                    target="_blank"
                    className="
              flex flex-col gap-2 p-4 
              border border-[#CFD8DC] rounded-[12px]
              hover:shadow-md hover:scale-[1.03]
              transition-all cursor-pointer
            "
                >
                    <img className="mx-auto" src={blog1} alt="" loading="lazy" />
                    <p className="">۱۰ کاربرد مهم گیج فشار روغنی در صنعت که باید بدانید!</p>
                </a>
            </ul>
        </div>
    );
}
