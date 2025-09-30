export default function GridHeader() {
  return (
    <div
      className="grid grid-cols-8 items-center border-b-2 border-[#CFD8DC] p-4 mb-2 text-center"
      role="row"
    >
      <span>انتخاب</span>
      <span>شناسه فرم</span>
      <span className="col-span-2">دسته بندی</span>
      <span>برند</span>
      <span>تعداد محصول</span>
      <span>آخرین به روز رسانی</span>
      <span>عملیات</span>
    </div>
  );
}
