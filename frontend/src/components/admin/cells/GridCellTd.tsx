// components/grid/GridCellTd.tsx
import { useState } from "react";
import { cn } from "@/lib/utils";
import { EditableCell } from "../cells/EditableCell";
import { ImageCell } from "../cells/ImageCell";
import { FileCell } from "../cells/FileCell";

type Props = {
  header: { type: string; key?: string; };
  cell: { id: number; value: string } | null;
  formId: string | null;
  current: string;
  rowCount?: number;
  moreIcon?: string;
};

export function GridCellTd({ header, cell, formId, current, rowCount, moreIcon }: Props) {
  const isImage = header.type === "Image";
  const isFile = header.type === "File";
  const isDesc = header.key === "description";
  const isPrice = header.key === "price";
  const isUnit = header.key === "unit";
  const isMore = header.type === "More";
  const isCheckbox = header.type === "Checkbox";
  const isRowno = header.type === "Rowno";
  const isStatic = isCheckbox || isRowno || isMore;

  // ✨ visual states
  const [savedFlash, setSavedFlash] = useState(false);
  const [errorFlash, setErrorFlash] = useState(false);
  const [sweep, setSweep] = useState(false);

  const onSaved = () => {
    // show sweep stripe + soft overlay
    setSweep(false); // restart the animation if retriggered quickly
    void Promise.resolve().then(() => setSweep(true));
    setSavedFlash(true);
    setTimeout(() => setSavedFlash(false), 700);
    setTimeout(() => setSweep(false), 700);
  };

  const onError = () => {
    setErrorFlash(true);
    setTimeout(() => setErrorFlash(false), 700);
  };

  return (
    <td
      className={cn(
        "relative group border p-2 text-center align-middle transition-[background,transform] duration-200",
        "border-[#3F414D] hover:bg-[#eceff1]",
        errorFlash && "shake-x"
      )}
    >
      {/* sweep success layer */}
      {sweep && <div className="absolute inset-0 pointer-events-none sweep-success rounded" />}

      {/* soft overlay flash */}
      {(savedFlash || errorFlash) && (
        <div
          className={cn(
            "absolute inset-0 pointer-events-none rounded opacity-100 transition-opacity duration-500",
            savedFlash && "flash-success",
            errorFlash && "flash-error"
          )}
        />
      )}

      {/* content on top */}
      <div className="z-10">
        {isCheckbox && <input type="checkbox" className="size-4 align-middle" />}
        {isRowno && <span>{rowCount}</span>}
        {isMore && moreIcon && <img className="m-auto" src={moreIcon} alt="بیشتر" />}

        {cell && (
          <>
            {isImage && <ImageCell cellId={cell.id} current={current} formId={formId} />}
            {isFile && <FileCell cellId={cell.id} current={current} formId={formId} />}
            {!isImage && !isFile && !isStatic && (
              <EditableCell
                cellId={cell.id}
                current={current}
                isDesc={isDesc}
                isPrice={isPrice}
                isUnit={isUnit}
                onSaved={onSaved}
                onError={onError}
              />
            )}
          </>
        )}

        {!cell && !isStatic && <span className="text-gray-400">—</span>}
      </div>
    </td>
  );
}
