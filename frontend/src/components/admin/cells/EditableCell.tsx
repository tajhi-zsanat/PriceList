// components/cells/EditableCell.tsx
import { Textarea } from "@/components/ui/textarea";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import check from "@/assets/img/admin/check_small.png";
import editIcon from "@/assets/img/admin/edit.png";
import { upsertCell } from "@/lib/api/formGrid";
import { toast } from "sonner";
import { useGridCtx } from "@/pages/admin/products/ctx/GridContext";
import { UNIT_OPTIONS } from "@/lib/utils";
import FarsiText from "@/components/FarsiText";
import FarsiInput, { parseFarsiNumber } from "@/components/FarsiInput";
import { Input } from "@/components/ui/input";

export function EditableCell({
  cellId, current, isDesc, isPrice, isUnit,
  onSaved, onError,
}: {
  cellId: number;
  current: string;
  isDesc?: boolean;
  isPrice?: boolean;
  isUnit?: boolean;
  onSaved?: () => void;
  onError?: () => void;
}) {
  const { editing, setEditing, editValue, setEditValue, setCellValues } = useGridCtx();
  const isEditing = editing === cellId;

  const startEdit = () => {
    // keep the raw string so FarsiInput can display Persian digits
    setEditing(cellId);
    setEditValue(current ?? "");
  };

  const normalizePrice = (s: string) => {
    const n = parseFarsiNumber(s); // -> number (English digits)
    return Number.isFinite(n) ? String(n) : "";
  };

  const save = async (v: string) => {
    const valueToSend = isPrice ? normalizePrice(v) : v;

    // optimistic UI
    setCellValues(p => ({ ...p, [cellId]: valueToSend }));
    setEditing(null);
    setEditValue("");

    try {
      await upsertCell({ id: cellId, value: valueToSend });
      toast.success("ذخیره شد.");
      onSaved?.();
    } catch {
      toast.error("ذخیره ناموفق بود.");
      onError?.();
    }
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    if (e.key === "Escape") setEditing(null);
    if (!isDesc && e.key === "Enter") {
      e.preventDefault();
      save(editValue);
    }
    if (isDesc && (e.key === "Enter") && (e.ctrlKey || e.metaKey)) {
      e.preventDefault();
      save(editValue);
    }
  };

  const formatFa = (raw: string | null | undefined) => {
    if (raw == null || raw === "") return "";
    const n = Number(raw);
    if (!Number.isFinite(n)) return String(raw ?? "");
    // Persian locale + group separators
    return n.toLocaleString("fa-IR");
  };

  return (
    <div
      className="w-full h-full"
      onClick={(e) => { e.stopPropagation(); startEdit(); }}
    >
      {!isEditing && (
        <button
          type="button"
          onClick={(e) => { e.stopPropagation(); startEdit(); }}
          className="absolute top-1 left-1 z-10 opacity-0 cursor-pointer group-hover:opacity-100 transition"
          title="ویرایش"
          aria-label="ویرایش"
        >
          <img src={editIcon} alt="" />
        </button>
      )}

      {isEditing && (
        <button
          type="button"
          onClick={() => save(editValue)}
          className="absolute cursor-pointer top-0 left-0 z-10"
          title="ذخیره"
          aria-label="ذخیره"
        >
          <img src={check} alt="" />
        </button>
      )}

      {isEditing ? (
        isDesc ? (
          <Textarea
            className="bg-white w-full my-2"
            autoFocus
            value={editValue}
            onChange={(e) => setEditValue(e.target.value)}
            onKeyDown={handleKeyDown}
            onBlur={() => save(editValue)}
          />
        ) : isUnit ? (
          <Select
            dir="rtl"
            value={editValue}
            onValueChange={(val) => { setEditValue(val); save(val); }}
          >
            <SelectTrigger 
            className="w-full text-center bg-white">
              <SelectValue placeholder="انتخاب واحد" />
            </SelectTrigger>
            <SelectContent>
              {UNIT_OPTIONS.map(u => <SelectItem key={u} value={u}>{u}</SelectItem>)}
            </SelectContent>
          </Select>
        ) : isPrice ? (
          <FarsiInput
            autoFocus
            className="bg-white"
            value={editValue === "" ? null : Number(editValue)}
            onChange={(n) => setEditValue(n == null ? "" : String(n))}
            onKeyDown={handleKeyDown}
            onBlur={() => save(editValue)}
          />
        ) : (
          <Input
            autoFocus
            className="bg-white"
            value={editValue}
            onChange={(e) => setEditValue(e.target.value)}
            onKeyDown={handleKeyDown}
            onBlur={() => save(editValue)}
          />
        )
      ) : (
        <span className="w-full block text-center truncate" title={String(current ?? "")}>
          {isPrice ? (
            <FarsiText>
              {current ? formatFa(current) : <span className="text-gray-400">—</span>}
            </FarsiText>
          ) : (
            current || <span className="text-gray-400">—</span>
          )}
        </span>
      )}
    </div>
  );
}
