import { useEffect, useMemo, useRef, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { Checkbox } from "@/components/ui/checkbox";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import { Textarea } from "@/components/ui/textarea";
import { toast } from "sonner";

import type { GridResponse, GridRow } from "@/types";
import { getGrid, uploadImage, upsertCell } from "@/lib/api/formGrid";
import { keyFor, requiredColIndexes } from "@/lib/grid-helpers";
import { resolveImgSrc } from "@/lib/helpers";
import moreIcon from "@/assets/img/more icon-mobile.png";

export default function Products() {
  const location = useLocation();
  const navigate = useNavigate();
  const formId = new URLSearchParams(location.search).get("formId");

  const [grid, setGrid] = useState<GridResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [err, setErr] = useState<string | null>(null);

  // local edit state
  const [editing, setEditing] = useState<{ row: number; col: number } | null>(null);
  const [editValue, setEditValue] = useState<string>("");

  // local value cache (optimistic)
  const [cellValues, setCellValues] = useState<Record<string, string>>({});
  const [files, setFiles] = useState<Record<string, File | null>>({});

  const hiddenFile = useRef<HTMLInputElement>(null);
  const [uploadTarget, setUploadTarget] = useState<{ r: number; c: number } | null>(null);

  useEffect(() => {
    if (!formId) {
      navigate("/admin/form");
      return;
    }

    const ctrl = new AbortController();
    setLoading(true);
    setErr(null);

    getGrid(formId, ctrl.signal)
      .then((data) => {
        if (!data) {
          navigate("/admin/form");
          return;
        }
        setGrid(data);
      })
      .catch((e: any) => {
        if (e?.name === "CanceledError" || e?.name === "AbortError") return;
        setErr(e?.message ?? "خطا در دریافت اطلاعات");
      })
      .finally(() => setLoading(false));

    return () => ctrl.abort();
  }, [formId, navigate]);

  const idx = useMemo(() => (grid ? requiredColIndexes(grid) : null), [grid]);

  // image upload handlers
  const onPickImage = (r: number, c: number) => {
    setUploadTarget({ r, c });
    hiddenFile.current?.click();
  };

  const onFileChange: React.ChangeEventHandler<HTMLInputElement> = (e) => {
    if (!uploadTarget) return;
    const f = e.target.files?.[0];
    if (!f) return;

    const k = keyFor(uploadTarget.r, uploadTarget.c);

    // cleanup old blob if any
    const prev = cellValues[k];
    if (prev && prev.startsWith("blob:")) {
      try {
        URL.revokeObjectURL(prev);
      } catch {
        /* no-op */
      }
    }

    const url = URL.createObjectURL(f);
    setFiles((p) => ({ ...p, [k]: f }));
    setCellValues((p) => ({ ...p, [k]: url })); // preview immediately
    e.currentTarget.value = "";
    setUploadTarget(null); // prevent stale target
  };

  const doUploadImage = async (id: number | undefined, r: number, c: number) => {
    if (!formId) {
      toast.error("شناسه فرم نامشخص است.");
      return;
    }
    if (typeof id !== "number") {
      toast.error("شناسه سلول نامعتبر است.");
      return;
    }

    const k = keyFor(r, c);
    const f = files[k];
    if (!f) return toast.warning("فایلی انتخاب نشده است.");

    try {
      const urlOrPath = await uploadImage(id, f); // e.g. "/uploads/..." or full URL
      if (urlOrPath) {
        const prev = cellValues[k];
        if (prev && prev.startsWith("blob:")) {
          try {
            URL.revokeObjectURL(prev);
          } catch {
            /* no-op */
          }
        }
        setCellValues((p) => ({ ...p, [k]: urlOrPath }));
        toast.success("تصویر با موفقیت بارگذاری شد.");
      } else {
        toast.success("تصویر درج شد.");
      }
    } catch {
      toast.error("خطا در بارگذاری تصویر.");
    }
  };

  const startEdit = (rowId: number, c: number, current: string) => {
    setEditing({ row: rowId, col: c });
    setEditValue(current ?? "");
  };

  const saveEdit = async (id: number | undefined, r: number, c: number, v: string) => {
    if (!formId) return;
    if (typeof id !== "number") {
      toast.error("شناسه سلول نامعتبر است.");
      return;
    }

    const k = keyFor(r, c);
    setCellValues((p) => ({ ...p, [k]: v })); // optimistic
    setEditing(null);
    setEditValue("");

    try {
      await upsertCell({ id, value: v });
      toast.success("ذخیره شد.");
    } catch {
      toast.error("ذخیره ناموفق بود.");
    }
  };

  if (loading) return <div className="p-4">در حال بارگذاری…</div>;
  if (err) return <div className="p-4 text-red-500">خطا: {err}</div>;
  if (!grid || !idx) return <div className="p-4">داده‌ای برای نمایش وجود ندارد.</div>;

  const totalCols = grid.headers.length;

  const renderRow = (row: GridRow, rIndex: number) => (
    <tr className="h-20" key={row.rowId}>
      {Array.from({ length: totalCols }).map((_, c) => {
        const k = keyFor(row.rowId, c);
        const cell = row.cells.find((x) => x.colIndex === c);

        // special columns by header type/key:
        const header = grid.headers[c];
        const isImage = header.type === "Image";
        const isFile = header.type === "File";
        const isDesc = header.key === "description";
        const isUnit = header.key === "unit";
        const isMore = header.type === "More";
        const isStatic = header.type === "Checkbox" || header.type === "Rowno" || isMore;

        if (header.type === "Checkbox") {
          return (
            <td key={c} className="border border-[#3F414D] p-2 text-center">
              <Checkbox />
            </td>
          );
        } else if (header.type === "Rowno") {
          return (
            <td key={c} className="border border-[#3F414D] p-2 text-center">
              {rIndex + 1}
            </td>
          );
        } else if (header.type === "More") {
          return (
            <td key={c} className="border border-[#3F414D] p-2 text-center ">
              <img className="m-auto" src={moreIcon} alt="بیشتر" />
            </td>
          );
        }

        if (!cell) {
          // No cell for this column — render an empty cell safely
          return (
            <td key={c} className="border border-[#3F414D] p-2 text-center ">
              <span className="text-gray-400">—</span>
            </td>
          );
        }

        const cellId = cell.id;
        const current = (cellValues[k] ?? cell?.value ?? "") as string;

        return (
          <td key={c} className="border border-[#3F414D] p-2 text-center hover:bg-[#eceff1] transition duration-200">
            {isImage && (
              <div className="flex items-center justify-center gap-2">
                {current ? (
                  <img src={resolveImgSrc(current)} className="w-12 h-12 object-cover rounded" alt="" />
                ) : null}
                <button type="button" className="underline" onClick={() => onPickImage(row.rowId, c)}>
                  بارگذاری
                </button>
                <button
                  type="button"
                  className="text-xs"
                  onClick={() => doUploadImage(cellId, row.rowId, c)}
                >
                  ارسال
                </button>
              </div>
            )}

            {isFile && <span className="text-xs text-gray-500">—</span>}

            {!isStatic && !isImage && !isFile && (
              editing?.row === row.rowId && editing?.col === c ? (
                isDesc ? (
                  <Textarea
                    className="bg-white"
                    autoFocus
                    value={editValue}
                    onChange={(e) => setEditValue(e.target.value)}
                    onKeyDown={(e) => {
                      if (e.key === "Escape") setEditing(null);
                      if (e.key === "Enter" && (e.ctrlKey || e.metaKey)) {
                        e.preventDefault();
                        saveEdit(cellId, row.rowId, c, editValue);
                      }
                    }}
                  />
                ) : isUnit ? (
                  <Select
                    value={editValue}
                    onValueChange={(val) => {
                      setEditValue(val);
                      saveEdit(cellId, row.rowId, c, val);
                    }}
                  >
                    <SelectTrigger className="w-full text-center bg-white">
                      <SelectValue placeholder="انتخاب واحد" />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="تعداد">تعداد</SelectItem>
                      <SelectItem value="بسته">بسته</SelectItem>
                      <SelectItem value="کیلوگرم">کیلوگرم</SelectItem>
                    </SelectContent>
                  </Select>
                ) : (
                  <input
                    autoFocus
                    className="w-full border rounded px-2 py-1 bg-white"
                    value={editValue}
                    onChange={(e) => setEditValue(e.target.value)}
                    onKeyDown={(e) => {
                      if (e.key === "Escape") setEditing(null);
                      if (e.key === "Enter") saveEdit(cellId, row.rowId, c, editValue);
                    }}
                  />
                )
              ) : (
                <button
                  type="button"
                  className="w-full text-center truncate"
                  onClick={() => startEdit(row.rowId, c, String(current ?? ""))}
                  title={String(current ?? "")}
                >
                  {String(current ?? "") || <span className="text-gray-400">—</span>}
                </button>
              )
            )}
          </td>
        );
      })}
    </tr>
  );

  return (
    <div className="flex-1">
      {/* shared hidden input for images */}
      <input
        ref={hiddenFile}
        type="file"
        accept="image/*"
        className="hidden"
        onChange={onFileChange}
      />

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
          <button
            type="button"
            className="flex items-center gap-2 bg-white text-[#636363] p-2 rounded-lg border border-[#636363] transition hover:bg-[#f2f5f7]"
          >
            <span>افزودن ویژگی</span>
          </button>
          <button
            type="button"
            className="flex items-center gap-2 bg-white text-[#636363] p-2 rounded-lg border border-[#636363] transition hover:bg-[#f2f5f7]"
          >
            <span>افزودن سرگروه</span>
          </button>
        </div>
      </div>

      <div>
        <table className="w-full table-fixed border border-[#CFD8DC] text-right border-collapse">
          <thead className="text-gray-700 font-medium">
            <tr className="h-20">
              {grid.headers.map((h) => {
                const isCustom = typeof h.type === "string" && h.type.includes("Custom");
                const isWide36 = h.type === "Image" || h.type === "Select" || h.type === "Price" || isCustom;
                const isWide24 = h.type === "File";
                const isWide48 = h.type === "MultilineText";

                // Option A: trust backend that 'Static' covers Checkbox/Rowno/More
                const isNarrow14 = h.kind === "Static";

                // Option B (safer): uncomment if you want type-based narrow cols too
                // const isNarrow14 = h.kind === "Static" || ["Checkbox", "Rowno", "More"].includes(h.type);

                return (
                  <th
                    key={h.index}            // or h.key ?? h.index if you have a stable key
                    scope="col"
                    className={[
                      "border border-[#3F414D] border-t-0 px-2 py-3 text-center",
                      isNarrow14 ? "w-14" : "",
                      isWide36 ? "w-36" : "",
                      isWide24 ? "w-24" : "",
                      isWide48 ? "w-48" : "",
                    ].join(" ").trim()}
                  >
                    {h.title}
                  </th>
                );
              })}

            </tr>
          </thead>
          <tbody>
            {grid.cells.length === 0 ? (
              <tr className="h-20">
                <td colSpan={totalCols} className="p-6 text-center text-gray-500">
                  ردیفی وجود ندارد
                </td>
              </tr>
            ) : (
              grid.cells.flatMap((group) => group.rows.map((row, i) => renderRow(row, i)))
            )}
          </tbody>
        </table>

        <div className="flex flex-col gap-2 border border-[#CFD8DC] border-t-0 rounded-b-[8px] p-3">
          <span className="text-[#636363]">توضیحات</span>
        </div>
      </div>
    </div>
  );
}
