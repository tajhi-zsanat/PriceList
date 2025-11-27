import addColIcon from "@/assets/img/admin/add-col.png"
import deleteicon from "@/assets/img/admin/delete-black.png"
import { AddRow, RemoveRow } from "@/lib/api/formGrid";
import { freezeScrollDuring } from "@/lib/utils";
import { useState } from "react";
import { toast } from "sonner";
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"

export function MoreCell({ moreIcon, featureId, formId, rowIndex, onAdded }: {
    moreIcon: string;
    featureId: number;
    rowIndex: number;
    formId: string | null;
    onAdded: () => Promise<void>;
}) {
    const [busy, setBusy] = useState(false);

    const addRow = async (featureId: number, rowIndex: number) => {
        if (busy) return;
        setBusy(true);
        try {
            await AddRow({ featureId, formId, rowIndex });
            toast.success("ردیف جدید اضافه گردید.");
            await freezeScrollDuring(onAdded);
        } catch {
            toast.error("ذخیره ناموفق بود.");
        } finally {
            setBusy(false);
        }
    };
    const removeRow = async (rowIndex: number) => {
        if (busy) return;
        setBusy(true);
        try {
            await RemoveRow({ formId, rowIndex });
            toast.success("ردیف حذف گردید.");
            await freezeScrollDuring(onAdded);
        } catch {
            toast.error("حذف ناموفق بود.");
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
                title="افزودن سطر"
                onClick={() => addRow(featureId, rowIndex)}
            />
            <DropdownMenu modal={false} dir="rtl">
                <DropdownMenuTrigger
                    className="flex justify-center items-center m-auto cursor-pointer">
                    <img
                        className="m-auto"
                        src={moreIcon} alt="بیشتر"
                    />
                </DropdownMenuTrigger>
                <DropdownMenuContent align="end">
                    <DropdownMenuItem
                        className="focus-visible:hidden"
                        onSelect={() => removeRow(rowIndex)}
                    >
                        <img
                            className=""
                            src={deleteicon} alt="حذف"
                        />
                        <span>حذف</span>
                    </DropdownMenuItem>
                </DropdownMenuContent>
            </DropdownMenu>
        </div>
    );
}