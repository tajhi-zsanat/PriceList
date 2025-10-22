import { keyFor } from "@/lib/helpers";
import { useGridCtx } from "@/pages/admin/products/ctx/GridContext";
import editIcon from "@/assets/img/admin/edit.png";
import check from "@/assets/img/admin/check_small.png";
import { toast } from "sonner";
import { upsertHeaderCell } from "@/lib/api/formGrid";


export function EditableHeader({
    formId,
    index,
    current
}: {
    formId: number;
    index: number;
    current: string;
}) {
    const { editingHeader, setEditingHeader, editValue, setEditValue, setcellValuesHeader } = useGridCtx();

    const headerCellId = keyFor(formId, index)
    const startEdit = () => { setEditingHeader(headerCellId); setEditValue(current ?? ""); };
    const save = async (v: string) => {
        setcellValuesHeader(p => ({ ...p, [headerCellId]: v }));
        setEditingHeader(null); setEditValue("");
        try { await upsertHeaderCell({ formId: formId, Index: index, value: v }); toast.success("ذخیره شد."); }
        catch { toast.error("ذخیره ناموفق بود."); }
    };

    const isEditing = editingHeader === headerCellId;

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
                <input autoFocus className="w-full border rounded px-2 py-1 bg-white"
                    value={editValue}
                    onChange={(e) => setEditValue(e.target.value)}
                    onKeyDown={(e) => {
                        if (e.key === "Escape") setEditingHeader(null);
                        if (e.key === "Enter") save(editValue);
                    }}
                    onBlur={() => save(editValue)}
                />
            ) : (
                <span className="w-full text-center truncate" title={String(current ?? "")}>
                    {String(current ?? "") || <span className="text-gray-400">—</span>}
                </span>
            )}
        </div>
    );
}