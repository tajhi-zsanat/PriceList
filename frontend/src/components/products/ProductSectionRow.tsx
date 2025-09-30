import type { SectionProps } from "@/types";

export default function ProductSectionRow({ title, color, colSpan }: SectionProps) {
    return (
        <tr style={{ background: color == null ? '#1F78AE' : color }}>
            <td className="text-white border border-gray-300 py-2 px-3" colSpan={colSpan}>{title}</td>
        </tr>
    );
}