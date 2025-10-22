// components/cells/EditableCell.tsx
import { Textarea } from "@/components/ui/textarea";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import check from "@/assets/img/admin/check_small.png";
import editIcon from "@/assets/img/admin/edit.png";
import { upsertCell } from "@/lib/api/formGrid";
import { toast } from "sonner";
import { useGridCtx } from "@/pages/admin/products/ctx/GridContext";
import { UNIT_OPTIONS } from "@/lib/utils";

export function EditableCell({
  cellId, current, isDesc, isUnit
}: {
  cellId: number; current: string; isDesc?: boolean; isUnit?: boolean;
}) {
  const { editing, setEditing, editValue, setEditValue, setCellValues } = useGridCtx();

  const startEdit = () => { setEditing(cellId); setEditValue(current ?? ""); };
  const save = async (v: string) => {
    setCellValues(p => ({ ...p, [cellId]: v }));
    setEditing(null); setEditValue("");
    try { await upsertCell({ id: cellId, value: v }); toast.success("ذخیره شد."); }
    catch { toast.error("ذخیره ناموفق بود."); }
  };

  const isEditing = editing === cellId;

  return (
    <div className="">
      {!isEditing && (
        <button type="button" onClick={(e) => { e.stopPropagation(); startEdit(); }}
          className="absolute top-1 left-1 z-10 opacity-0 cursor-pointer group-hover:opacity-100 transition"
          title="ویرایش" aria-label="ویرایش">
          <img src={editIcon} alt="" />
        </button>
      )}

      {isEditing && (
        <button type="button" onClick={() => save(editValue)}
          className="absolute cursor-pointer top-0 left-0 z-10" title="ذخیره" aria-label="ذخیره">
          <img src={check} alt="" />
        </button>
      )}

      {isEditing ? (
        isDesc ? (
          <Textarea className="bg-white w-full" autoFocus
            value={editValue}
            onChange={(e) => setEditValue(e.target.value)}
            onKeyDown={(e) => {
              if (e.key === "Escape") setEditing(null);
              if (e.key === "Enter" && (e.ctrlKey || e.metaKey)) { e.preventDefault(); save(editValue); }
            }}
            onBlur={() => save(editValue)}
          />
        ) : isUnit ? (
          <Select value={editValue} onValueChange={(val) => { setEditValue(val); save(val); }}>
            <SelectTrigger className="w-full text-center bg-white">
              <SelectValue placeholder="انتخاب واحد" />
            </SelectTrigger>
            <SelectContent>
              {UNIT_OPTIONS.map(u => <SelectItem key={u} value={u}>{u}</SelectItem>)}
            </SelectContent>
          </Select>
        ) : (
          <input autoFocus className="w-full border rounded px-2 py-1 bg-white"
            value={editValue}
            onChange={(e) => setEditValue(e.target.value)}
            onKeyDown={(e) => {
              if (e.key === "Escape") setEditing(null);
              if (e.key === "Enter") save(editValue);
            }}
            onBlur={() => save(editValue)}
          />
        )
      ) : (
        <span className="w-full text-center truncate" title={String(current ?? "")}>
          {String(current ?? "") || <span className="text-gray-400">—</span>}
        </span>
      )}
    </div>
  );
}
