import type { FormListItemDto } from "@/types";
import GridHeader from "./GridHeader";
import Row from "./Row";

export default function FormsTable({ data }: { data: FormListItemDto[] }) {
  return (
    <div className="flex flex-col gap-1 mt-4" role="table" aria-label="لیست فرم‌ها">
      <GridHeader />
      <div className="flex flex-col gap-1 mt-2" role="rowgroup">
        {data.map((f, i) => (
          <Row key={f.id} index={i} item={f} />
        ))}
      </div>
    </div>
  );
}
