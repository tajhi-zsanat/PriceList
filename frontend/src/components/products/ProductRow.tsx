// ProductRow.tsx
import FarsiText from "@/components/FarsiText";
import moreIcon from "../../assets/img/more icon-mobile.png";
import pdfProduct from "../../assets/img/admin/download-PDF_new_new_new.png";
import noImage from '@/assets/img/no-image.png';
import { resolveImgSrc } from "@/lib/helpers";
import type { FormHeader, GridCell } from "@/types";

type Row = {
    rowId: number;
    rowCount: number;
    rowIndex: number;
    cells: GridCell[];
};

type ProductRowProps = {
    row: Row;
    headers: FormHeader[];
};

export default function ProductRow({ row, headers }: ProductRowProps) {
    const cellMap = new Map<number, GridCell>();
    for (const c of row.cells) {
        cellMap.set(c.colIndex, c);
    }

    const renderCellContent = (h: FormHeader) => {
        const cell = cellMap.get(h.index);
        const value = cell?.value ?? "";

        switch (h.type) {
            case "Checkbox":
                return;
            // return (
            //     <input
            //         type="checkbox"
            //         className="w-4 h-4 accent-[#1F78AE]"
            //     />
            // );

            case "Rowno":
                return row.rowCount;

            case "Image": {
                return (
                    <img
                        className="m-auto max-h-16"
                        src={!value ? noImage : resolveImgSrc(value)}
                        alt="عکس محصول"
                    />
                );
            }

            case "File": {
                if (!value) return "--";
                const href = resolveImgSrc(value);
                return (
                    <a
                        href={href}
                        target="_blank"
                        rel="noopener noreferrer"
                        className="flex justify-center"
                    >
                        <img
                            src={pdfProduct}
                            alt="فایل محصول"
                        />
                    </a>
                );
            }

            case "MultilineText":
                return value ? <FarsiText><span className="text-base">{value}</span></FarsiText> : "--";

            case "Select":
                // واحد
                return value || "--";

            case "Custom1":
            case "Custom2":
            case "Custom3":
                return value || "--";

            case "Price": {
                if (!value) return "--";
                const num = Number(value);
                if (Number.isNaN(num)) return value;
                return (
                    <span className="text-[#2A7906] font-bold text-base">
                        <FarsiText>{num.toLocaleString()}</FarsiText>
                    </span>
                );
            }

            case "More":
                return (
                    <img
                        className="m-auto cursor-pointer"
                        src={moreIcon}
                        alt="بیشتر"
                    />
                );

            default:
                return value || "--";
        }
    };

    return (
        <tr>
            {headers.map((h) => {
                if (h.type === "Checkbox") return null;

                return (
                    <td
                        key={`${row.rowId}-col-${h.index}`}
                        className="border border-gray-300 px-2 py-4 text-center align-middle"
                    >
                        {renderCellContent(h)}
                    </td>
                );
            })}
        </tr>
    );
}
