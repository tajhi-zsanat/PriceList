// ProductsPage.tsx
import { useLocation, useNavigate } from "react-router-dom";
import { useEffect } from "react";
import { useGridData } from "./hooks/useGridData";
import { GridProvider, useGridCtx } from "./ctx/GridContext";
import { GridTable } from "@/components/admin/GridTable";
import AddColDefModal from "@/components/admin/products/AddColDefModal";
import backwardIcon from "@/assets/img/admin/chevron_backward.png";
import forwardIcon from "@/assets/img/admin/chevron_forward.png";
import AddGroupModal from "@/components/admin/products/AddGroupModal";
import FarsiText from "@/components/FarsiText";

function ProductsInner() {
  const location = useLocation();
  const navigate = useNavigate();
  const formId = new URLSearchParams(location.search).get("formId");

  const { grid, loading, err, nextPage, prevPage, refetch } = useGridData(formId);
  const { cellValues, cellValuesHeader } = useGridCtx();

  useEffect(() => {
    if (!formId) navigate("/admin/form", { replace: true });
  }, [formId, navigate]);

  if (!formId) return null;

  if (loading) return <div className="p-4">در حال بارگذاری…</div>;
  if (err) return <div className="p-4 text-red-500">خطا: {err}</div>;
  if (!grid) return <div className="p-4">در حال بارگذاری...</div>;

  return (
    <div className="flex-1">
      <div className="flex justify-between items-center py-6">
        <div className="flex flex-col gap-2">
          <div className="flex items-center gap-2">
            <h3>{grid.formTitle}</h3>
            <span className="text-sm">
              ({grid.meta.totalRows} محصول)
            </span>
          </div>
          <h6 className="text-base text-[#636363]">فرم <FarsiText>{formId}</FarsiText></h6>
        </div>
        <div className="flex items-center gap-2">
          <button type="button" className="button-outline">
            <span>بایگانی کردن فرم</span>
          </button>
          <button
            type="button"
            className="flex items-center gap-2 bg-[#CFE2FF] p-2 rounded-lg border border-transparent transition hover:border-[#1F78AE]"
          >
            <span>وارد سازی فایل</span>
          </button>
          <button
            type="button"
            className="flex items-center gap-2 bg-[#1F78AE] text-white p-2 rounded-lg border border-transparent transition hover:bg-[#0f4566]"
          >
            <span>خروجی اکسل</span>
          </button>
        </div>
      </div>

      <div className="border border-[#3F414D] rounded-tl-[4px] rounded-tr-[4px]">
        <div className="flex items-center justify-between p-2">
          <div className="flex items-center gap-2">
            <AddGroupModal
              cells={grid.cells}
              formId={formId}
              onCreated={refetch}
              trigger={
                <button
                  type="button"
                  className="flex items-center gap-2 bg-white text-[#000000] p-2 rounded-lg border border-[#000000] cursor-pointer transition hover:bg-[#f2f5f7]"
                >
                  <span>انتخاب ویژگی</span>
                </button>
              }
            />
            <AddColDefModal
              formId={Number(formId)}
              currentCount={grid.headers.length}
              onCreated={refetch}
              trigger={
                <button
                  type="button"
                  className="flex items-center gap-2 bg-white text-[#000000] p-2 rounded-lg border border-[#000000] cursor-pointer transition hover:bg-[#f2f5f7]"
                >
                  <span>افزودن سرگروه</span>
                </button>
              }
            />
          </div>
          <div className="flex justify-center items-center gap-2">
            <span>
              صفحه {grid.meta.page} از {grid.meta.totalPages}
            </span>
            <button
              onClick={prevPage}
              disabled={!grid.meta.hasPrev}
            >
              <img
                className={`${!grid.meta.hasPrev ? 'opacity-50' : "cursor-pointer"}`}
                src={forwardIcon}
                alt="بعدی" />
            </button>
            <button
              onClick={nextPage}
              disabled={!grid.meta.hasNext}
            >
              <img
                className={`${!grid.meta.hasNext ? 'opacity-50' : "cursor-pointer"}`}
                src={backwardIcon}
                alt="قبلی" />
            </button>
          </div>
        </div>
      </div>

      <GridTable
        grid={grid}
        formId={formId}
        cellValues={cellValues}
        cellValuesHeader={cellValuesHeader}
        onDeleted={refetch}
      />
      <div className="flex flex-col gap-2 border border-[#CFD8DC] border-t-0 rounded-b-[8px] p-3">
        <span className="text-[#636363]">توضیحات: </span>
        <p className="text-base font-medium">{grid.formTitle}</p>
      </div>
    </div>
  );
}

export default function Products() {
  return (
    <GridProvider>
      <ProductsInner />
    </GridProvider>
  );
}
