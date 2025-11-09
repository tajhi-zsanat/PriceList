// components/GridGroup.tsx
import type { FormHeader, GridGroup } from "@/types";
import { GridRow } from "./GridRow";

export function GridGroup({ group, headers, gi, formId, cellValues, featureId, onAdded }: {
  group: GridGroup;
  headers: FormHeader[];
  gi: number;
  formId: string | null;
  cellValues: Record<number, string>;
  featureId: number;
  onAdded: () => Promise<void>;
}) {
  const totalCols = headers.length;

  return (
    <>
      {
        <tr className={`h-8 border border-black`} style={{ backgroundColor: `${group.color}` }}>
          <td colSpan={totalCols} className="px-4 py-3 text-right border-none">
            <div className="flex items-center justify-between">
              <span className="font-medium text-white">
                {group.featureName}
              </span>
              <span className="text-xs text-white">{group.count} مورد</span>
            </div>
          </td>
        </tr>
      }

      {(group.rows ?? []).map((row: any) => (
        <GridRow
          key={`g-${gi}-r-${row.rowId}`}
          row={row}
          headers={headers}
          formId={formId}
          cellValues={cellValues}
          featureId={featureId}
          onAdded={onAdded}
        />
      ))}
    </>
  );
}
