export function ProductsHeader() {
  return (
    <div className="flex justify-between items-center py-6">
      <h3>فرم 343</h3>
      <div className="flex items-center gap-2">
        <button type="button" className="button-outline"><span>بایگانی کردن فرم</span></button>
        <button type="button" className="flex items-center gap-2 bg-[#CFE2FF] p-2 rounded-lg border transition hover:border-[#1F78AE] cursor-pointer">
          <span>وارد سازی فایل</span>
        </button>
        <button type="button" className="flex items-center gap-2 bg-[#1F78AE] text-white p-2 rounded-lg border transition hover:bg-[#0f4566] cursor-pointer">
          <span>خروجی اکسل</span>
        </button>
      </div>
    </div>
  );
}
