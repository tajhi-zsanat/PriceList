import type { FormHeader } from "@/types";
import { EditableHeader } from "./cells/EditableHeader";
import { keyFor } from "@/lib/helpers";

export function GridHeaderRow({ headers, cellValuesHeader, onDeleted }: {
  headers: FormHeader[],
  cellValuesHeader: Record<string, string>,
  onDeleted: () => Promise<void>;
}) {
  return (
    <tr className="h-20">
      {headers.map((h) => {
        const isCustom = typeof h.type === "string" && h.type.includes("Custom");
        const isWide48 = h.type === "Image" || h.type === "Select" || h.type === "Price" || isCustom;
        const isWide24 = h.type === "File";
        const isNarrow14 = h.kind === "Static";

        const current = (cellValuesHeader[keyFor(h.formId, h.index)] ?? h.title ?? "") as string;
        return (
          <th key={h.index} scope="col"
            className={[
              isCustom ? "relative hover:bg-[#eceff1] transition group" : "",
              "border border-[#3F414D] border-t-0 px-2 py-3 text-center",
              isNarrow14 ? "w-14" : "", isWide48 ? "w-48" : "", isWide24 ? "w-24" : ""
            ].join(" ").trim()}>

            {isCustom && (
              <EditableHeader
                key={`r-${h.index}-i${h.index}`}
                formId={h.formId} index={h.index}
                current={current}
                onDeleted={onDeleted}
              />
            )}

            {!isCustom && (
              current
            )}
          </th>
        );
      })}
    </tr>
  );
}
