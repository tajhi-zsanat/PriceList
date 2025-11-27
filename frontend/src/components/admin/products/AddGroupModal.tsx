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
import type { GetRowNumberList } from "@/types";
import { Label } from "@radix-ui/react-label";
import { Input } from "@/components/ui/input";
import { addFeatureToRows, GetFormRowOrder } from "@/lib/api/formGrid";
import {
    MultiSelect,
    type MultiSelectOption,
} from "@/components/ui/multi-select";

type Props = {
    trigger: React.ReactNode;
    formId: number | string;
    onCreated?: () => void;
};

export default function AddGroupModal({ trigger, formId, onCreated }: Props) {
    const [values, setValues] = useState<string[]>([]); // selected ids as string[]
    const [data, setData] = useState<GetRowNumberList[]>([]);
    const [options, setOptions] = useState<MultiSelectOption[]>([]);
    const [open, setOpen] = useState(false);
    const [loading, setLoading] = useState(false);
    const [err, setErr] = useState<string | null>(null);

    const [feature, setFeature] = useState<string>("");
    const [displayOrder, setDisplayOrder] = useState<string>("");
    const [color, setColor] = useState<string>("#206E4E");

    const palette = ["#206E4E", "#805F00", "#AE2E23", "#5E4DB3", "#1F78AE"];

    const fetchFormRowOrder = async (signal: AbortSignal) => {
        try {
            const result = await GetFormRowOrder({ formId, signal });
            setData(result);
        } catch {
            toast.error("بارگذاری ردیف‌ها ناموفق بود.");
        }
    };

    useEffect(() => {
        if (!open) return;

        const ctrl = new AbortController();
        fetchFormRowOrder(ctrl.signal);

        return () => ctrl.abort();
    }, [open, formId]);

    useEffect(() => {
        if (!data || data.length === 0) {
            setOptions([]);
            setValues([]);
            return;
        }

        const opts: MultiSelectOption[] = data.map((row) => ({
            label: row.featureName
                ? `ردیف ${row.rowNumber} - ${row.featureName}`
                : `ردیف ${row.rowNumber}`,
            value: row.id.toString(),
            // optional style per row:
            // style: {
            //   badgeColor: "#1F78AE",
            // },
        }));

        setOptions(opts);

        // setValues(opts.map(o => o.value));
    }, [data]);

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        setErr(null);
        if (values.length == 0) { toast.warning("ردیف انتخاب نشده است."); return; }
        if (feature == "") { toast.warning("ویژگی وارد نشده است."); return; }
        if (!displayOrder || displayOrder === "") { toast.warning("ترتیب نمایش باید بزرگ تر از 0 باشد."); return; }

        setLoading(true);
        try {
            const ctrl = new AbortController();
            await addFeatureToRows({
                formId,
                feature: feature,
                rowIds: values,
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
                        <Label className="text-bold">ردیف های انتخابی</Label>
                        <MultiSelect
                            autoSize={true}
                            maxCount={1}
                            placeholder="انتخاب ردیف"
                            options={options}
                            value={values}
                            onValueChange={setValues}
                            modalPopover={true}
                        />
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
                            {loading ? "در حال ثبت..." : "ثبت ویژگی"}
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
