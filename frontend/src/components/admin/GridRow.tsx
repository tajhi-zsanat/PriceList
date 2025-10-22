import { ImageCell } from "./cells/ImageCell";
import { FileCell } from "./cells/FileCell";
import { EditableCell } from "./cells/EditableCell";
import { Checkbox } from "@/components/ui/checkbox";
import moreIcon from "@/assets/img/more icon-mobile.png";
import type { FormHeader, GridRow } from "@/types";

export function GridRow({ row, headers, rIndex, formId, cellValues }:{
  row: GridRow; headers: FormHeader[]; rIndex: number; formId: string | null; cellValues: Record<number,string>;
}) {
  const totalCols = headers.length;
  return (
    <tr className="h-20" key={row.rowId}>
      {Array.from({ length: totalCols }).map((_, c) => {
        const cell = row.cells.find((x:any) => x.colIndex === c);
        const header = headers[c];
        const isImage = header.type === "Image";
        const isFile = header.type === "File";
        const isDesc = header.key === "description";
        const isUnit = header.key === "unit";
        const isMore = header.type === "More";
        const isStatic = header.type === "Checkbox" || header.type === "Rowno" || isMore;

        if (header.type === "Checkbox")
          return <td key={c} className="border border-[#3F414D] p-2 text-center"><Checkbox/></td>;

        if (header.type === "Rowno")
          return <td key={c} className="border border-[#3F414D] p-2 text-center">{rIndex+1}</td>;

        if (isMore)
          return <td key={c} className="border border-[#3F414D] p-2 text-center"><img className="m-auto" src={moreIcon} alt="بیشتر"/></td>;

        if (!cell)
          return <td key={c} className="border border-[#3F414D] p-2 text-center"><span className="text-gray-400">—</span></td>;

        const cellId = cell.id;
        const current = (cellValues[cellId] ?? cell?.value ?? "") as string;

        return (
          <td key={c} className="relative group border border-[#3F414D] p-2 text-center align-middle hover:bg-[#eceff1] transition">
            {isImage && <ImageCell cellId={cellId} current={current} formId={formId} />}
            {isFile && <FileCell cellId={cellId} current={current} formId={formId} />}
            {!isImage && !isFile && !isStatic && (
              <EditableCell cellId={cellId} current={current} isDesc={isDesc} isUnit={isUnit} />
            )}
          </td>
        );
      })}
    </tr>
  );
}
