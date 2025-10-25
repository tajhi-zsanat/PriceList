import { GridHeaderRow } from "./GridHeaderRow";
import { GridGroup } from "./GridGroup";
import type { GridResponse } from "@/types";

export function GridTable({ grid, formId, cellValues, cellValuesHeader, onDeleted }: {
  grid: GridResponse;
  formId: string | null;
  cellValues: Record<number, string>;
  cellValuesHeader: Record<string, string>;
  onDeleted: () => Promise<void>;
}) {
  const totalCols = grid.headers.length;
  return (
    <table className="w-full table-fixed border border-[#CFD8DC] text-right border-collapse">
      <thead className="text-gray-700 font-medium">
        <GridHeaderRow
          headers={grid.headers}
          cellValuesHeader={cellValuesHeader}
          onDeleted={onDeleted}
        />
      </thead>
      <tbody>
        {grid.cells.length === 0 ? (
          <tr className="h-8">
            <td colSpan={totalCols} className="p-6 text-center text-gray-500">ردیفی وجود ندارد</td>
          </tr>
        ) : (
          grid.cells.map((group: any, gi: number) => (
            <GridGroup key={`g-${gi}`} group={group} gi={gi} headers={grid.headers}
              formId={formId} cellValues={cellValues} />
          ))
        )}
      </tbody>
    </table>
  );
}
