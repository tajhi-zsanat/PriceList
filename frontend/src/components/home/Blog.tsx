import arrowLeft from "@/assets/img/home/arrrow-left-link.png";
import { blogData } from "@/Data/blog";

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
                {blogData.map((b) => (
                    <li key={b.id}>
                        <a
                            href={b.href}
                            target="_blank"
                            rel="noopener noreferrer"
                            className="
          flex flex-col gap-3 h-full pb-4
          border border-[#CFD8DC] rounded-[12px]
          hover:shadow-md hover:scale-[1.03]
          transition-all cursor-pointer bg-white
        "
                        >
                            <div className="w-full flex justify-center">
                                <img
                                    src={b.src}
                                    alt={b.name}
                                    loading="lazy"
                                    className="object-cover rounded-tr-[12px] rounded-tl-[12px]"
                                />
                            </div>

                            <p className="text-base font-medium text-[#3F414D] px-2">{b.name}</p>
                        </a>
                    </li>
                ))}
            </ul>
        </div>
    );
}
