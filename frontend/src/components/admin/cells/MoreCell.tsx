import addColIcon from "@/assets/img/admin/add-col.png"
import { AddRow } from "@/lib/api/formGrid";
import { freezeScrollDuring } from "@/lib/utils";
import { useState } from "react";
import { toast } from "sonner";

export function MoreCell({ moreIcon, featureId, formId, rowIndex, onAdded }: {
    moreIcon: string;
    featureId: number;
    rowIndex: number;
    formId: string | null;
    onAdded: () => Promise<void>;
}) {
    const [busy, setBusy] = useState(false);
    debugger;
    const addRow = async (featureId: number, rowIndex: number) => {
        if (busy) return;
        setBusy(true);
        try {
            await AddRow({ featureId, formId, rowIndex });
            toast.success("ردیف جدید اضافه گردید.");
            await freezeScrollDuring(onAdded); // ⬅️ no flash
        } catch {
            toast.error("ذخیره ناموفق بود.");
        } finally {
            setBusy(false);
        }
    };
    return (
        <div>
            <img
                className="absolute -bottom-1.5 left-0 z-10 cursor-pointer
                 transition-all duration-300 ease-out opacity-0 group-hover:opacity-100"
                src={addColIcon}
                onClick={() => addRow(featureId, rowIndex)}
            />
            <img
                className="m-auto"
                src={moreIcon} alt="بیشتر"
            />
        </div>
    );
}