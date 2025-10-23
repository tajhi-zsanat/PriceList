// ProductsPage.tsx
import { useLocation, useNavigate } from "react-router-dom";
import { useEffect, useMemo } from "react";
import { useGridData } from "./hooks/useGridData";
import { GridProvider, useGridCtx } from "./ctx/GridContext";
import { requiredColIndexes } from "@/lib/grid-helpers";
import { GridTable } from "@/components/admin/GridTable";
import AddColDefModal from "@/components/products/AddColDefModal";

function ProductsInner() {
  const location = useLocation();
  const navigate = useNavigate();
  const formId = new URLSearchParams(location.search).get("formId");

  const { grid, loading, err, refetch } = useGridData(formId);
  const { cellValues, cellValuesHeader } = useGridCtx();

  useEffect(() => {
    if (!formId) navigate("/admin/form", { replace: true });
  }, [formId, navigate]);

  const idx = useMemo(() => (grid ? requiredColIndexes(grid) : null), [grid]);

  if (!formId) return null;

  if (loading) return <div className="p-4">در حال بارگذاری…</div>;
  if (err) return <div className="p-4 text-red-500">خطا: {err}</div>;
  if (!grid || !idx) return <div className="p-4">در حال بارگذاری...</div>;

  return (
    <div className="flex-1">
      <div className="flex justify-between items-center py-6">
        <h3>فرم 343</h3>
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
        <div className="flex items-center gap-2 p-2">
          <AddColDefModal
            formId={Number(formId)}
            currentCount={grid.headers.length} 
            onCreated={refetch}
            trigger={
              <button
                type="button"
                className="flex items-center gap-2 bg-white text-[#636363] p-2 rounded-lg border border-[#636363] cursor-pointer transition hover:bg-[#f2f5f7]"
              >
                <span>افزودن سرگروه</span>
              </button>
            }
          />
          <button
            type="button"
            className="flex items-center gap-2 bg-white text-[#636363] p-2 rounded-lg border border-[#636363] transition hover:bg-[#f2f5f7]"
          >
            <span>افزودن ویژگی</span>
          </button>
        </div>
      </div>
      <GridTable grid={grid} formId={formId} cellValues={cellValues} cellValuesHeader={cellValuesHeader} />
      <div className="flex flex-col gap-2 border border-[#CFD8DC] border-t-0 rounded-b-[8px] p-3">
        <span className="text-[#636363]">توضیحات</span>
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
