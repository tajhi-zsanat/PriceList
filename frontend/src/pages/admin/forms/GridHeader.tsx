export default function GridHeader() {
  return (
    <div
      className="grid grid-cols-13 items-center border-b-2 border-[#CFD8DC] py-5 mb-2 text-center"
      role="row"
    >
      <span className="col-span-1">انتخاب</span>
      <span className="col-span-1">شناسه فرم</span>
      <span className="col-span-3">دسته بندی</span>
      <span className="col-span-3">برند</span>
      <span className="col-span-1">ردیف</span>
      <span className="col-span-3">آخرین به روز رسانی</span>
      <span className="col-span-1">عملیات</span>
    </div>
  );
}
