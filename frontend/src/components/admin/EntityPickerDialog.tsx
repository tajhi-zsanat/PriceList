// components/common/EntityPickerDialog.tsx
import { useState, useEffect } from "react";
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import type { BaseItem, EntityPickerDialogProps } from "@/types";
import { DialogDescription } from "@radix-ui/react-dialog";
import beforeIcon from "@/assets/img/admin/navigate_before.png";
// import loadingIcon from "@/assets/img/loading.gif";


export default function EntityPickerDialog<TItem extends BaseItem>({
    open,
    onOpenChange,
    title,
    loadItems,
    onSelect,
    placeholder = "جستجو",
    renderRow,
}: EntityPickerDialogProps<TItem>) {
    const [q, setQ] = useState("");
    const [items, setItems] = useState<TItem[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    // simple debounce
    const [debouncedQ, setDebouncedQ] = useState(q);
    useEffect(() => {

        const id = setTimeout(() => setDebouncedQ(q), 300);
        return () => clearTimeout(id);
    }, [q]);

    useEffect(() => {
        if (!open) return;

        const controller = new AbortController();
        let mounted = true;
        debugger;
        setLoading(true);
        setError(null);
        loadItems({ search: debouncedQ, signal: controller.signal })
            .then((data) => mounted && setItems(data))
            .catch((e) => {
                // swallow abort errors; show others
                if (e?.name === "AbortError" || e?.name === "CanceledError") return;
                setError(e?.response?.data ?? e?.message ?? "خطایی رخ داد");
            })
            .finally(() => {
                mounted
                    && setLoading(false)
            });

        return () => {
            mounted = false;
            controller.abort();
        };
    }, [open, debouncedQ, loadItems]);

    // reset search when reopened
    useEffect(() => {
        if (open) setQ("");
    }, [open]);

    return (
        <Dialog open={open} onOpenChange={onOpenChange}>
            <DialogContent dir="rtl" className="">
                <DialogHeader className="text-start">
                    <DialogTitle className="border-b border-b-[#CFD8DC] pb-4">{title}</DialogTitle>
                    <DialogDescription className="text-sm text-muted-foreground mt-1">
                        لطفاً آیتم مورد نظر خود را جستجو و انتخاب کنید.
                    </DialogDescription>
                </DialogHeader>

                <div className="grid gap-2 mb-3">
                    <Input
                        className="bg-[#F5F5F5]"
                        placeholder={placeholder}
                        value={q}
                        onChange={(e) => setQ(e.target.value)}
                    />
                </div>

                {/* {loading && <div className="py-2 text-sm text-muted-foreground">
                    <img src={loadingIcon} alt="" aria-hidden="true" />
                </div>} */}
                {error && <div className="py-2 text-sm text-red-600">{error}</div>}

                <div className="flex flex-col h-64 max-h-64 overflow-auto">
                    {items.map((item) => (
                        <button
                            type="button"
                            key={item.id}
                            className="flex justify-between items-center border-b border-b-[#CFD8DC] py-3 text-start cursor-pointer"
                            onClick={() => {
                                onSelect(item);
                                onOpenChange(false);
                            }}
                        >
                            {renderRow ? renderRow(item) : <span>{item.name}</span>}
                            <img src={beforeIcon} alt="" aria-hidden="true" />
                        </button>
                    ))}
                    {!loading && !error && items.length === 0 && (
                        <div className="py-6 text-center text-sm text-muted-foreground">نتیجه‌ای یافت نشد.</div>
                    )}
                </div>
            </DialogContent>
        </Dialog>
    );
}
