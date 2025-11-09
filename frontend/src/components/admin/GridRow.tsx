// components/grid/GridRow.tsx
import moreIcon from "@/assets/img/more icon-mobile.png";
import type { FormHeader, GridRow as GridRowType } from "@/types";
import { GridCellTd } from "./cells/GridCellTd";

export function GridRow({
  row, headers, formId, cellValues, featureId, onAdded
}: {
  row: GridRowType;
  headers: FormHeader[];
  formId: string | null;
  cellValues: Record<number, string>;
  featureId: number;
  onAdded: () => Promise<void>;
}) {
  const totalCols = headers.length;
  
  return (
    <tr className="h-20" key={row.rowId}>
      {Array.from({ length: totalCols }).map((_, c) => {
        const header = headers[c];
        const cell = row.cells.find((x: any) => x.colIndex === c) ?? null;
        const current = cell ? (cellValues[cell.id] ?? cell.value ?? "") as string : "";

        return (
          <GridCellTd
            key={c}
            header={header}
            cell={cell && { id: cell.id, value: current }}
            formId={formId}
            current={current}
            rowCount={row.rowCount}
            rowIndex={row.rowIndex}
            moreIcon={moreIcon}
            featureId={featureId}
            onAdded={onAdded}
          />
        );
      })}
    </tr>
  );
}
