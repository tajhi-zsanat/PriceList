import { useRef, useState } from "react";
import pdfIcon from "@/assets/img/admin/download-PDF_new_new.png";
import uploadIcon from "@/assets/img/admin/upload-PDF_new_new.png";
import deleteIcon from "@/assets/img/admin/delete.png";
import check from "@/assets/img/admin/check_small.png";
import { resolveImgSrc, isBlobUrl } from "@/lib/helpers";
import { useGridCtx } from "@/pages/admin/products/ctx/GridContext";
import { useFileUpload } from "@/pages/admin/products/hooks/useFileUpload";

export function FileCell({ cellId, current, formId }: {
  cellId: number; current: string; formId: string | null;
}) {
  const inputRef = useRef<HTMLInputElement>(null);
  const { files, setFiles, cellValues, setCellValues } = useGridCtx();
  const { setPreview, doUploadPDF, removeMedia } = useFileUpload(formId);
  const [isBusy, setIsBusy] = useState(false);

  const onPick = () => inputRef.current?.click();
  const onChange: React.ChangeEventHandler<HTMLInputElement> = (e) => {
    const f = e.target.files?.[0]; if (!f) return;
    setPreview(cellId, f);
    e.currentTarget.value = "";
  };
  const cancelPending = () => {
    const prev = cellValues[cellId];
    setFiles(p => { const c = { ...p }; delete c[cellId]; return c; });
    if (isBlobUrl(prev)) {
      setCellValues(p => { const c = { ...p }; delete c[cellId]; return c; });
    }
  };

  return (
    <div className="w-16 h-16 flex items-center justify-center m-auto">
      <input ref={inputRef} type="file" accept="application/pdf" className="hidden" onChange={onChange} />
      {current ? (
        <a className="m-auto" target="_blank" href={resolveImgSrc(current)}>
          <img src={pdfIcon} className="m-auto cursor-pointer" alt="دانلود PDF" />
        </a>
      ) : !files[cellId] && (
        <button type="button" className="m-auto" onClick={onPick} title="انتخاب فایل">
          <img src={uploadIcon} alt="" className="cursor-pointer" />
        </button>
      )}

      {files[cellId] && (
        <>
          <button type="button" onClick={() => doUploadPDF(cellId, setIsBusy)} disabled={isBusy}
            className={`absolute top-1 left-1 ${isBusy ? 'opacity-50' : ''}`} title="ارسال">
            <img src={check} alt="send" className="cursor-pointer" />
          </button>
          <button type="button" onClick={cancelPending}
            className="absolute top-1 right-1" title="لغو">
            <img src={deleteIcon} alt="cancel" className="cursor-pointer" />
          </button>
        </>
      )}

      {current && !files[cellId] && (
        <button type="button" onClick={() => removeMedia(cellId, current)}
          className="absolute top-1 left-1 opacity-0 group-hover:opacity-100 transition" title="حذف">
          <img src={deleteIcon} alt="delete" className="cursor-pointer" />
        </button>
      )}
    </div>
  );
}
