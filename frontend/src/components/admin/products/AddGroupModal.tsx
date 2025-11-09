import { useEffect, useState } from "react";
import {
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
    DialogFooter,
    DialogTrigger,
    DialogClose,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { toast } from "sonner";
import type { GridGroup } from "@/types";
import { useGridCtx } from "@/pages/admin/products/ctx/GridContext";
import { Label } from "@radix-ui/react-label";
import { Input } from "@/components/ui/input";
import { uniq } from "@/lib/helpers";
import { addFeatureToRows } from "@/lib/api/formGrid";

type Props = {
    trigger: React.ReactNode;
    cells: GridGroup[];
    formId: number | string;
    onCreated?: () => void;
};

export default function AddGroupModal({ trigger, cells, formId, onCreated }: Props) {
    const { rowIds, setRowIds } = useGridCtx();

    const [open, setOpen] = useState(false);
    const [loading, setLoading] = useState(false);
    const [err, setErr] = useState<string | null>(null);

    const [feature, setFeature] = useState<string>("");
    const [startrow, setStartrow] = useState<number>();
    const [displayOrder, setDisplayOrder] = useState<string>("");
    const [endrow, setendrow] = useState<number>();
    const [color, setColor] = useState<string>("#206E4E");

    const palette = ["#206E4E", "#805F00", "#AE2E23", "#5E4DB3", "#1F78AE"];

    useEffect(() => {
        fillRowIds(cells);
    }, [cells]);

    const fillRowIds = (groups: GridGroup[]) => {
        const map: Record<number, number> = {};
        groups.forEach(g => {
            g.rows.forEach(r => {
                map[r.rowCount] = r.rowId;
            });
        });

        setRowIds(map);
    };

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        setErr(null);

        if (startrow == null) { toast.warning("از ردیف وارد نشده است."); return; }
        if (endrow == null) { toast.warning("تا ردیف وارد نشده است."); return; }
        if (endrow < startrow) { toast.warning("تا ردیف باید بزرگتر یا مساوی از ردیف باشد."); return; }
        if (feature == "") { toast.warning("ویژگی وارد نشده است."); return; }
        if (!displayOrder || displayOrder === "") { toast.warning("ترتیب نمایش باید بزرگ تر از 0 باشد."); return; }

        setLoading(true);
        try {
            const range = Array.from({ length: endrow - startrow + 1 }, (_, i) => startrow + i);

            const selectedRowIDs = uniq(
                range.map((rc) => rowIds[rc]).filter((v): v is number => typeof v === "number")
            );

            if (selectedRowIDs.length === 0) {
                toast.warning("هیچ ردیفی با این محدوده یافت نشد.");
                return;
            }

            const ctrl = new AbortController();
            await addFeatureToRows({
                formId,
                feature: feature,
                rowIds: selectedRowIDs,
                displayOrder,
                color,
            }, ctrl.signal);

            toast.success("دسته‌بندی با موفقیت ثبت شدند.");
            onCreated?.();
            setOpen(false);
        } catch (e: any) {
            const msg = e?.response?.data || e?.message || "خطای نامشخص رخ داد";
            setErr(msg);
        } finally {
            setLoading(false);
        }
    }

    return (
        <Dialog open={open} onOpenChange={setOpen}>
            <DialogTrigger asChild>{trigger}</DialogTrigger>
            <DialogContent dir="rtl" className="sm:max-w-lg w-full">
                <DialogHeader className="text-start gap-4">
                    <DialogTitle className="border-b border-b-[#CFD8DC] pb-4">
                        افزودن ویژگی جدید
                    </DialogTitle>
                </DialogHeader>

                <form onSubmit={handleSubmit} className="space-y-4">
                    {err && <p className="text-red-500 text-sm">{err}</p>}

                    <div className="flex-1 grid gap-2">
                        <Label htmlFor="displayOrder" className="text-bold">انتخاب دسته‌بندی</Label>
                        <Input
                            id="FeatureName"
                            type="text"
                            value={feature ?? ""}
                            placeholder="نام ویژگی"
                            onChange={(e) => {
                                const v = e.currentTarget.value;
                                setFeature(v);
                            }}
                        />
                    </div>
                    <div className="flex-1 grid gap-2">
                        <Label htmlFor="displayOrder" className="text-bold">ترتیب نمایش</Label>
                        <Input
                            id="displayOrder"
                            type="number"
                            min={1}
                            value={displayOrder}
                            placeholder="ترتیب نمایش"
                            onChange={(e) => {
                                const v = e.target.value;
                                setDisplayOrder(v);
                            }}
                        />
                    </div>
                    <div className="flex-1 grid gap-2">
                        <Label className="">محدوده ردیف ها</Label>
                        <div className="flex flex-col md:flex-row justify-between items-center gap-2">
                            <div className="flex-1 grid gap-2">
                                <Input
                                    id="startIndex"
                                    type="number"
                                    min={1}
                                    value={startrow ?? ""}
                                    placeholder="از ردیف"
                                    onChange={(e) => {
                                        const v = e.currentTarget.valueAsNumber;
                                        setStartrow(Number.isNaN(v) ? undefined : v);
                                    }}
                                />
                            </div>
                            <div className="flex-1 grid gap-2">
                                <Input
                                    id="endIndex"
                                    type="number"
                                    min={1}
                                    value={endrow ?? ""}
                                    placeholder="تا ردیف"
                                    onChange={(e) => {
                                        const v = e.currentTarget.valueAsNumber;
                                        setendrow(Number.isNaN(v) ? undefined : v);
                                    }}
                                />
                            </div>
                        </div>
                    </div>
                    <div>

                        <span className="block mb-2">انتخاب رنگ</span>
                        <div className="flex items-center gap-2 flex-wrap">
                            {palette.map((c) => {
                                const isActive = c.toLowerCase() === color.toLowerCase();
                                return (
                                    <button
                                        key={c}
                                        type="button"
                                        onClick={() => setColor(c)}
                                        className={`w-8 h-8 rounded-md border transition
                     focus:outline-none focus:ring-2 focus:ring-offset-2
                     ${isActive ? "ring-2 ring-offset-2" : ""}`}
                                        style={{ backgroundColor: c }}
                                        aria-label={`color ${c}`}
                                    />
                                );
                            })}

                            {/* free-form color */}
                            <label className="ml-2 inline-flex items-center gap-2 text-sm">
                                <span>سفارشی</span>
                                <input
                                    type="color"
                                    value={color}
                                    onChange={(e) => setColor(e.target.value)}
                                    className="h-8 w-10 p-0 border rounded-md"
                                />
                            </label>
                        </div>
                    </div>
                    <DialogFooter className="gap-2 sm:gap-2">
                        <Button
                            className="flex-1 bg-[#1F78AE]"
                            type="submit"
                            disabled={loading}
                        >
                            {loading ? "در حال ثبت..." : "ثبت دسته‌بندی"}
                        </Button>
                        <DialogClose asChild>
                            <Button
                                className="flex-1 border border-[#1F78AE]"
                                type="button"
                                variant="outline"
                                disabled={loading}
                            >
                                انصراف
                            </Button>
                        </DialogClose>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog >
    );
}
