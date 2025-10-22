import { toast } from "sonner";
import { uploadImage, uploadPDF, RemoveCellMedia } from "@/lib/api/formGrid";
import { isBlobUrl } from "@/lib/helpers";
import { useGridCtx } from "../ctx/GridContext";

export function useFileUpload(formId: string | null) {
  const { files, setFiles, cellValues, setCellValues } = useGridCtx();

  const clearPendingFor = (id: number) => {
    const prev = cellValues[id];
    if (isBlobUrl(prev)) { try { URL.revokeObjectURL(prev); } catch {}
    }
    setFiles(p => { const c = { ...p }; delete c[id]; return c; });
  };

  const setPreview = (id: number, f: File) => {
    const prev = cellValues[id];
    if (isBlobUrl(prev)) { try { URL.revokeObjectURL(prev); } catch {} }
    const url = URL.createObjectURL(f);
    setFiles(p => ({ ...p, [id]: f }));
    setCellValues(p => ({ ...p, [id]: url }));
  };

  const doUploadImage = async (id: number) => {
    if (!formId) return toast.error("شناسه فرم نامشخص است.");
    const f = files[id]; if (!f) return toast.warning("فایلی انتخاب نشده است.");
    const sizeMB = f.size / (1024 * 1024);
    if (sizeMB > 2) return toast.warning("سایز تصویر بیش از ۲MB است.");

    try {
      const url = await uploadImage(id, f);
      const prev = cellValues[id];
      if (isBlobUrl(prev)) { try { URL.revokeObjectURL(prev); } catch {} }
      setCellValues(p => ({ ...p, [id]: url }));
      clearPendingFor(id);
      toast.success("تصویر بارگذاری شد.");
    } catch { toast.error("خطا در بارگذاری تصویر."); }
  };

  const doUploadPDF = async (id: number) => {
    debugger;
    if (!formId) return toast.error("شناسه فرم نامشخص است.");
    const f = files[id]; if (!f) return toast.warning("فایلی انتخاب نشده است.");
    try {
      const url = await uploadPDF(id, f);
      const prev = cellValues[id];
      if (isBlobUrl(prev)) { try { URL.revokeObjectURL(prev); } catch {} }
      setCellValues(p => ({ ...p, [id]: url }));
      clearPendingFor(id);
      toast.success("فایل بارگذاری شد.");
    } catch { toast.error("خطا در بارگذاری فایل."); }
  };

  const removeMedia = async (id: number, current: string | null) => {
    if (!formId) return toast.error("شناسه فرم نامشخص است.");
    if (files[id]) clearPendingFor(id); // لغو آپلود در صف
    const prev = cellValues[id];
    setCellValues(p => ({ ...p, [id]: "" })); // خوش‌بینانه
    try {
      if (!isBlobUrl(prev)) await RemoveCellMedia({ id, value: current });
      if (isBlobUrl(prev)) { try { URL.revokeObjectURL(prev!); } catch {} }
      toast.success("حذف شد.");
    } catch {
      setCellValues(p => ({ ...p, [id]: prev ?? "" }));
      toast.error("حذف ناموفق.");
    }
  };

  return { setPreview, doUploadImage, doUploadPDF, removeMedia };
}
