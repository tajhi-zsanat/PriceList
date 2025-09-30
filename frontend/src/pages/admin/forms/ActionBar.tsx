import box from '@/assets/img/admin/box.png';
import percent from '@/assets/img/admin/percent.png';
import arrow from '@/assets/img/admin/arrow_white.png';

export default function ActionBar() {
  return (
    <div className="flex items-center justify-between">
      <h3>فرم های من</h3>
      <div className="flex items-center gap-3">
        <button type="button" className="button-outline">
          <img src={box} alt="" aria-hidden="true" />
          <span>بایگانی فرم</span>
        </button>

        <button
          type="button"
          className="flex items-center gap-2 bg-[#CFE2FF] p-2 rounded-lg border border-transparent transition hover:border-[#1F78AE]"
        >
          <img src={percent} alt="" aria-hidden="true" className="w-5 h-5" />
          <span>ثبت تخفیف گروهی</span>
        </button>

        <button
          type="button"
          className="flex items-center gap-2 bg-[#1F78AE] text-white p-2 rounded-lg border border-transparent transition hover:bg-[#0f4566]"
        >
          <img src={arrow} alt="" aria-hidden="true" className="w-5 h-5" />
          <span>افزایش گروهی مبلغ</span>
        </button>
      </div>
    </div>
  );
}
