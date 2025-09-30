import type { PriceToolbarProps } from "@/types";
import bell from "../../assets/img/bell.png";
import pdf from "../../assets/img/pdf.png";
import print from "../../assets/img/print.png";
import { Switch } from "@/components/ui/switch";


export default function PriceToolbar({ onPdf, onPrint, onNotify, vatChecked, onVatChange }: PriceToolbarProps) {
    return (
        <div className="flex justify-between items-center">
            <div className="flex items-center gap-4">
                <button className="button-outline" onClick={onPdf}>
                    <img src={pdf} alt="pdf" />
                    <span>خروجی PDF</span>
                </button>
                <button className="button-outline" onClick={onPrint}>
                    <img src={print} alt="print" />
                    <span>چاپ PDF</span>
                </button>
                <button className="button-outline" onClick={onNotify}>
                    <img src={bell} alt="bell" />
                    <span>خبرم کن</span>
                </button>
            </div>
            <div className="flex items-center gap-2">
                <Switch className="[direction:ltr] data-[state=checked]:bg-[#1F78AE] data-[state=unchecked]:bg-gray-300"
                    checked={vatChecked}
                    onCheckedChange={onVatChange} />
                <span>ارزش افزوده</span>
            </div>
        </div>
    );
}