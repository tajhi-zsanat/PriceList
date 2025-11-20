// ProductHeader.tsx
import { Fragment } from "react";
import ProductSectionRow from "./ProductSectionRow";
import ProductRow from "./ProductRow";
import type { FormHeader, GridGroup } from "@/types";

interface ProductHeaderProps {
  formHeaders: FormHeader[];
  groupCells: GridGroup[];
  formName: string;
}

export default function ProductHeader({ formHeaders, groupCells, formName }: ProductHeaderProps) {
  const headers = formHeaders ?? [];

  return (
    <div className="overflow-x-auto mx-8 mb-4">
      <table className="min-w-full border border-[#CFD8DC] text-right border-collapse">
        <thead className="bg-[#CFE2FF] text-gray-700 font-medium">
          <tr>
            {headers.map((h) => {
              if (h.type == "Checkbox") return;
              const isCustom = typeof h.type === "string" && h.type.includes("Custom");

              const isWide48 = h.type === "Image" || h.type === "Select" || h.type === "Price" || isCustom;
              const isWide24 = h.type === "File";
              const isDes = h.type === "MultilineText";
              const isNarrow14 = h.kind === "Static";

              const widthClass = isWide48
                ? "w-48"
                : isWide24
                  ? "w-24"
                  : isDes
                    ? "w-96"
                    : isNarrow14
                      ? "w-14"
                      : "w-auto";

              return (
                <th
                  key={h.index}
                  className={`border border-[#1F78AE] px-2 py-6 text-center ${widthClass}`}
                >
                  {h.title}
                </th>
              );
            })}

          </tr>
        </thead>
        <tbody>
          {groupCells.map((g) => (
            <Fragment key={g.featureId}>
              <ProductSectionRow
                title={g.featureName}
                color={g.color}
                colSpan={headers.length}
              />

              {g.rows.map((r) => (
                <ProductRow
                  key={r.rowId}
                  row={r}
                  headers={headers}
                />
              ))}
            </Fragment>
          ))}
        </tbody>
      </table>

      <div className="flex flex-col gap-2 border border-[#CFD8DC] border-t-0 rounded-b-[8px] p-3">
        <span className="text-[#636363]">توضیحات</span>
        <span>{formName}</span>
      </div>
    </div>
  );
}
