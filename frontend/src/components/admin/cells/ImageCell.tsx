import { useRef } from "react";
import { resolveImgSrc, isBlobUrl } from "@/lib/helpers";
import check from "@/assets/img/admin/check_small.png";
import uploadIcon from "@/assets/img/admin/solar_upload-linear.png";
import deleteIcon from "@/assets/img/admin/delete.png";
import { useGridCtx } from "@/pages/admin/products/ctx/GridContext";
import { useFileUpload } from "@/pages/admin/products/hooks/useFileUpload";

export function ImageCell({ cellId, current, formId }: {
  cellId: number; current: string; formId: string | null;
}) {
  const inputRef = useRef<HTMLInputElement>(null);
  const { files, setFiles, cellValues, setCellValues } = useGridCtx();
  const { setPreview, doUploadImage, removeMedia } = useFileUpload(formId);

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
      <input ref={inputRef} type="file" accept="image/*" className="hidden" onChange={onChange} />
      {current ? (
        <img
          src={resolveImgSrc(current)}
          className="w-full h-full object-contain rounded cursor-pointer m-auto"
          alt="" onClick={onPick}
        />
      ) : !files[cellId] && (
        <button type="button" className="m-auto cursor-pointer" onClick={onPick} title="انتخاب تصویر">
          <img className="cursor-pointer" src={uploadIcon} alt="" />
        </button>
      )}

      {files[cellId] && (
        <>
          <button type="button" onClick={() => doUploadImage(cellId)}
            className="absolute top-1 left-1" title="ارسال">
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
          <img src={deleteIcon} alt="delete" className="w-full h-full object-cover rounded cursor-pointer" />
        </button>
      )}
    </div>
  );
}
