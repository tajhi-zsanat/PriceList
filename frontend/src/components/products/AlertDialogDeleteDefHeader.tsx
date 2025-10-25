import { useState } from "react";
// ✅ shadcn/ui
import {
    AlertDialog,
    AlertDialogAction,
    AlertDialogCancel,
    AlertDialogContent,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogHeader,
    AlertDialogTitle,
    AlertDialogTrigger,
} from "@/components/ui/alert-dialog";
import deleteIcon from "@/assets/img/admin/delete.png";
import { removeDef } from "@/lib/api/formGrid";
import { toast } from "sonner";
import { useGridCtx } from "@/pages/admin/products/ctx/GridContext";


export function AlertDialogDeleteDefHeader({
    formId,
    index,
    onDeleted
}: {
    formId: number;
    index: number;
    onDeleted: () => Promise<void>;
}) {
    const { setcellValuesHeader } = useGridCtx();
    const [confirmOpen, setConfirmOpen] = useState(false);
    const [deleting, setDeleting] = useState(false);

    const handleConfirmDelete = async () => {
        setDeleting(true);
        try {
            await removeDef({ formId, index });
            setcellValuesHeader({});
            await onDeleted();
            toast.success("ستون حذف شد.");
            setConfirmOpen(false);
        } catch {
            toast.error("حذف ناموفق بود.");
        } finally {
            setDeleting(false);
        }
    };
    return (
        <AlertDialog open={confirmOpen} onOpenChange={setConfirmOpen}>
            <AlertDialogTrigger asChild>
                <button
                    type="button"
                    onClick={(e) => {
                        // prevent cell click side-effects
                        e.stopPropagation();
                        setConfirmOpen(true)
                    }}
                    className="absolute top-1 right-1 z-10 opacity-0 cursor-pointer group-hover:opacity-100 transition"
                    title="حذف ستون"
                    aria-label="حذف ستون"
                    disabled={deleting}
                >
                    <img
                        src={deleteIcon}
                        alt="حذف"
                        className={`cursor-pointer ${deleting ? "opacity-50" : ""}`}
                    />
                </button>
            </AlertDialogTrigger>

            <AlertDialogContent dir="rtl">
                <AlertDialogHeader>
                    <AlertDialogTitle>حذف سرستون</AlertDialogTitle>
                    <AlertDialogDescription>
                        آیا از حذف این سرستون مطمئن هستید؟ این کار قابل بازگشت نیست و
                        داده‌های مرتبط با این ستون نیز حذف می‌شوند.
                    </AlertDialogDescription>
                </AlertDialogHeader>
                <AlertDialogFooter>
                    <AlertDialogCancel disabled={deleting}>
                        انصراف
                    </AlertDialogCancel>
                    <AlertDialogAction
                        onClick={(e) => {
                            e.preventDefault();
                            handleConfirmDelete();
                        }}
                        disabled={deleting}
                    >
                        {deleting ? "در حال حذف..." : "حذف کن"}
                    </AlertDialogAction>
                </AlertDialogFooter>
            </AlertDialogContent>
        </AlertDialog>
    );
}