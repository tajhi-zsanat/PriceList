// components/GridGroup.tsx
import type { FormHeader, GridGroup } from "@/types";
import { GridRow } from "./GridRow";

export function GridGroup({ group, headers, gi, formId, cellValues }: {
  group: GridGroup; headers: FormHeader[]; gi: number; formId: string | null; cellValues: Record<number, string>;
}) {
  const totalCols = headers.length;
  return (
    <>
      {Array.isArray(group.featureNames) && (
        <tr className={`h-8 border border-black`} style={{ backgroundColor: `${group.featureColor}`  }}>
          <td colSpan={totalCols} className="px-4 py-3 text-right border-none">
            <div className="flex items-center justify-between">
              <span className="font-medium text-white">
                {group.featureNames.length === 0 ? "بدون ویژگی" : group.featureNames.join(" - ")}
              </span>
              <span className="text-xs text-white">{group.rows?.length ?? 0} مورد</span>
            </div>
          </td>
        </tr>
      )}

      {(group.rows ?? []).map((row: any) => (
        <GridRow key={`g-${gi}-r-${row.rowId}`} row={row}
          headers={headers} formId={formId} cellValues={cellValues} />
      ))}
    </>
  );
}
