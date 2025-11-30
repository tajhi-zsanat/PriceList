import landing2 from "@/assets/img/home/company.png";
import checkIcon from "@/assets/img/home/check.png";
import plusIcon from "@/assets/img/home/plus.png";

export default function IntroduceCompany() {
  return (
    <div className="max-w-6xl m-auto shadow-[0_0_16px_0_#D9D9D9] p-8 rounded-[20px] my-4" dir="rtl">
      <div className="flex gap-6">
        <img src={landing2} alt="" className="h-auto rounded-xl object-cover" />
    
        <div className="flex flex-col gap-4 flex-1">
          <p className="text-[22px] font-medium text-[#3F414D]">معرفی شرکت</p>

          <div className="flex items-center gap-3">
            <button
              className="
                min-w-[120px] h-[40px]
                bg-[#1F78AE] text-white 
                font-medium rounded-[8px] 
                px-4 cursor-pointer 
                flex items-center justify-center
                transition hover:bg-[#18648f]
              "
            >
              پیکاتک
            </button>

            <button
              className="
                min-w-[120px] h-[40px]
                bg-white text-[#3F414D] 
                border border-[#3F414D] 
                font-medium rounded-[8px]
                px-4 cursor-pointer
                flex items-center justify-center
                transition hover:bg-gray-100
              "
            >
              تجهیز صنعت
            </button>

            <button
              className="
                min-w-[120px] h-[40px]
                bg-white text-[#3F414D] 
                border border-[#3F414D] 
                font-medium rounded-[8px]
                px-4 cursor-pointer
                flex items-center justify-center
                transition hover:bg-gray-100
              "
            >
              پیکا اکسپرس
            </button>
          </div>

          <p className="text-[#3F414D] text-base leading-7">
            پیکاتک با شعار رشد و توسعه کسب و کارهای صنعتی یک سامانه تخصصی خرید و فروش آنلاین محصولات صنعتی می‌باشد.
          </p>

          <ul className="grid grid-cols-2 gap-3 mt-2">
            {[
              "تنوع گسترده محصولات صنعتی",
              "پرداخت امن و مطمئن",
              "ضمانت اصل کالا",
              "پشتیبانی تخصصی",
            ].map((item, i) => (
              <li key={i} className="flex items-center gap-2">
                <img src={checkIcon} alt="" className="w-5 h-5" />
                <span className="text-base font-medium">{item}</span>
              </li>
            ))}
          </ul>
        </div>
      </div>

      <ul className="grid grid-cols-4 gap-6 mt-8">
        {[
          { number: "500", text: "محصول متنوع در حوزه تجهیزات صنعتی" },
          { number: "50", text: "برند کالاهای صنعتی" },
          { number: "10", text: "سال تجربه در فروش تجهیزات صنعتی" },
          { number: "500", text: "مشتری وفادار در صنایع مختلف" },
        ].map((item, i) => (
          <li key={i} className="flex flex-col items-center justify-center">
            <div className="flex items-center gap-2">
              <img src={plusIcon} alt="" className="w-6 h-6" />
              <span className="text-[58px] text-[#1F78AE] leading-none">
                {item.number}
              </span>
            </div>
            <p className="text-balance font-medium text-[#3F414D] mt-1">{item.text}</p>
          </li>
        ))}
      </ul>
    </div>
  );
}
