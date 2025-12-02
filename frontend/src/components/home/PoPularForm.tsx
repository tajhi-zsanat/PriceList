import boxIcon from "@/assets/img/home/icon_box.png";
import timeIcon from "@/assets/img/home/icon_time.png";
import noImage from '@/assets/img/no-image.svg';
import { imgUrl } from "@/lib/helpers";
import type { PopularFormDto } from "@/types";
import loadingImage from '@/assets/img/loading.gif';
import FarsiText from "../FarsiText";
import { Link } from "react-router-dom";

type PopularFormProps = {
  data: PopularFormDto[];
  loading: boolean;
};

export default function PopularForm({ data, loading }: PopularFormProps) {
  if (!data) return;
  return (
    <div className="mb-4 max-w-6xl m-auto" dir="rtl">
      <p className="text-[22px] font-medium mb-6">پربازدیدترین لیست‌های قیمتی</p>
      {loading && <div>
        <img
          className=""
          src={loadingImage}
          alt='Loading'
          loading="lazy"
        />
      </div>}
      <ul className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">

        {data.map((item) => (
          <Link to={`/Category/${item.categoryId}/groups/${item.productGroupId}/brands/${item.brandId}/products?formId=${item.id}`}
            key={item.id}
            className="
              flex flex-col gap-2 p-4 
              border border-[#CFD8DC] rounded-[12px]
              hover:shadow-md hover:scale-[1.03]
              transition-all cursor-pointer bg-white
            "
          >
            <div className="w-full h-[140px] flex justify-center">
              <img
                className="w-full h-full object-contain"
                src={item.imagePath ? imgUrl(item.imagePath) : noImage}
                alt={item.formTitle}
              />
            </div>

            <p className="text-base font-medium text-[#3F414D]">
              {item.formTitle}
            </p>

            <p className="text-[#636363]">
              کد تأمین‌کننده: {item.id}
            </p>

            <div className="flex items-center gap-2 text-[#636363]">
              <img className="w-5" src={boxIcon} alt="محصولات" />
              <span>تعداد محصولات: {<FarsiText>{item.rowCountWithDescriptionAndPrice}</FarsiText>} عدد</span>
            </div>

            <div className="flex items-center gap-2 text-[#636363]">
              <img className="w-5" src={timeIcon} alt="زمان" />
              <span>آخرین به‌روزرسانی: {<FarsiText>{item.updateDate}</FarsiText>}</span>
            </div>
          </Link>
        ))}

      </ul>
    </div>
  );
}
