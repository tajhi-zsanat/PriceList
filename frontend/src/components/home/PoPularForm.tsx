import boxIcon from "@/assets/img/home/icon_box.png";
import timeIcon from "@/assets/img/home/icon_time.png";
import { mockData } from "@/mockData/home";

export default function PoPularForm() {
  return (
    <div className="mt-28 mb-4 max-w-6xl m-auto" dir="rtl">
      <p className="text-[22px] font-medium mb-4">پربازدیدترین لیست‌های قیمتی</p>

      <ul className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">

        {mockData.map((item) => (
          <li
            key={item.id}
            className="
              flex flex-col gap-2 p-4 
              border border-[#CFD8DC] rounded-[12px]
              hover:shadow-md hover:scale-[1.03]
              transition-all cursor-pointer bg-white
            "
          >
            <img className="mx-auto" src={item.brandIcon} alt={item.name} />

            <p className="text-base font-medium text-[#3F414D]">
              {item.name}
            </p>

            <p className="text-[#636363]">
              کد تأمین‌کننده: {item.supplierCode}
            </p>

            <div className="flex items-center gap-2 text-[#636363]">
              <img className="w-5" src={boxIcon} alt="محصولات" />
              <span>تعداد محصولات: {item.productCount} عدد</span>
            </div>

            <div className="flex items-center gap-2 text-[#636363]">
              <img className="w-5" src={timeIcon} alt="زمان" />
              <span>آخرین به‌روزرسانی: {item.lastUpdate}</span>
            </div>
          </li>
        ))}

      </ul>
    </div>
  );
}
