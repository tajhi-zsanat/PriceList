import { useEffect, useMemo, useState } from "react"
import {
    Dialog,
    DialogContent,
    DialogHeader,
    DialogTitle,
    DialogFooter,
    DialogTrigger,
    DialogClose,
} from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { AddColDef } from "@/lib/api/formGrid"
import { toast } from "sonner";

type Props = {
    trigger: React.ReactNode
    formId: number
    currentCount: number 
    onCreated?: () => void
}

const MIN_COLS = 8
const MAX_COLS = 11
const MAX_CUSTOM = 3

export default function AddColDefModal({ trigger, formId, currentCount, onCreated }: Props) {
    const [open, setOpen] = useState(false)
    const [loading, setLoading] = useState(false)
    const [err, setErr] = useState<string | null>(null)

    // How many customs already exist (0..3)
    const existingCustoms = useMemo(
        () => Math.min(MAX_CUSTOM, Math.max(0, currentCount - MIN_COLS)),
        [currentCount]
    )

    // Which custom numbers we should render now (e.g., [2,3] when current=9)
    const customNumbers = useMemo(() => {
        const remain = Math.max(0, MAX_CUSTOM - existingCustoms) // 0..3
        return Array.from({ length: remain }, (_, i) => existingCustoms + 1 + i)
    }, [existingCustoms])

    const [titles, setTitles] = useState<string[]>([])
    useEffect(() => {
        setTitles(Array.from({ length: customNumbers.length }, () => ""))
    }, [customNumbers.length])

    const setTitleAt = (i: number, v: string) => {
        setTitles((prev) => {
            const next = [...prev]
            next[i] = v
            return next
        })
    }

    const nothingToAdd = currentCount >= MAX_COLS || customNumbers.length === 0

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        setErr(null);
        setLoading(true);
        try {
            const normalized = titles.map((t) => t.trim());
            await AddColDef({ formId, customColDef: normalized });
            toast.success("سر گروه با موفقیت ایجاد شد.");
            onCreated?.();
            setOpen(false);
        } catch (e: any) {
            // Axios error message
            const msg = e?.response?.data || e?.message || "خطای نامشخص رخ داد";
            setErr(msg);
        } finally {
            setLoading(false);
        }
    }

    return (
        <Dialog open={open} onOpenChange={setOpen}>
            {/* If at max, render a disabled trigger; else use as real trigger */}
            {nothingToAdd ? (
                <button
                    type="button"
                    className="flex items-center gap-2 bg-gray-100 text-gray-400 p-2 rounded-lg border border-gray-300 cursor-not-allowed"
                    title="حداکثر تعداد ستون‌ها (11) تکمیل است"
                    disabled
                >
                    <span>افزودن ویژگی</span>
                </button>
            ) : (
                <DialogTrigger asChild>{trigger}</DialogTrigger>
            )}

            <DialogContent dir="rtl" className="sm:max-w-lg w-full">
                <DialogHeader className="text-start gap-4">
                    <DialogTitle className="border-b border-b-[#CFD8DC] pb-4">
                        افزودن ویژگی جدید
                    </DialogTitle>
                </DialogHeader>

                <p className="text-[#636363] text-base">
                    میتوانید حداکثر سه ویژگی برای فرم خود وارد نمایید.
                </p>
                <form onSubmit={handleSubmit} className="space-y-4">
                    {customNumbers.map((num, i) => (
                        <div key={num}>
                            <label className="block text-sm text-[#3F414D] mb-1">
                                عنوان ویژگی {num}
                            </label>
                            <input
                                type="text"
                                value={titles[i] ?? ""}
                                onChange={(e) => setTitleAt(i, e.target.value)}
                                placeholder={`مثلاً: ویژگی ${num}`}
                                className="w-full border border-[#CFD8DC] rounded-lg p-2 outline-none focus:ring-2 focus:ring-[#1F78AE]"
                            />
                        </div>
                    ))}

                    {err && <p className="text-red-500 text-sm">{err}</p>}

                    <DialogFooter className="gap-2 sm:gap-2">
                        <Button
                            className="flex-1 bg-[#1F78AE]"
                            type="submit"
                            disabled={loading}
                        >
                            {loading ? "در حال ثبت..." : "ثبت ویژگی"}
                        </Button>
                        <DialogClose asChild>
                            <Button className="flex-1 border border-[#1F78AE]"
                                type="button"
                                variant="outline"
                            >
                                انصراف
                            </Button>
                        </DialogClose>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    )
}
