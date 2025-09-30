import type { ProductTableProps } from "@/types";
import ProductSectionRow from "./ProductSectionRow";
import ProductRow from "./ProductRow";

export default function ProductTable({ buckets, headers }: ProductTableProps) {
    let rowNo = 0;
    return (
        <div className="overflow-x-auto mx-8 mb-4">
            <table className="min-w-full border border-[#CFD8DC] text-right border-collapse">
                <thead className="bg-[#CFE2FF] text-gray-700 font-medium">
                    <tr>
                        <th className="border border-[#1F78AE] px-2 py-6 text-center w-16">ردیف</th>
                        <th className="border border-[#1F78AE] px-2 py-6 text-center">عکس</th>
                        <th className="border border-[#1F78AE] px-2 py-6 text-center">فایل</th>
                        <th className="border border-[#1F78AE] px-2 py-6 text-center">شرح کالا</th>
                        {headers?.map(h => (
                            <th key={h.id} className="border border-[#1F78AE] px-2 py-6 text-center">{h.value}</th>
                        ))}
                        <th className="border border-[#1F78AE] px-2 py-6 text-center">واحد</th>
                        <th className="border border-[#1F78AE] px-2 py-6 text-center">قیمت</th>
                        <th className="border border-[#1F78AE] text-center w-8" />
                    </tr>
                </thead>
                <tbody>
                    {buckets.map(b => (
                        <>
                            <ProductSectionRow key={`sec-${b.featuresIDs}`} title={b.title} color={b.featureColor} colSpan={7 + (headers?.length ?? 0)} />
                            {b.products.map(p => { rowNo += 1; return <ProductRow key={p.id} index={rowNo} product={p} headers={headers} />; })}
                        </>
                    ))}
                </tbody>
            </table>
            <div className="flex flex-col gap-2 border border-[#CFD8DC] border-t-0 rounded-b-[8px] p-3">
                <span className="text-[#636363]">توضیحات</span>
                <span>فرم گیج فشار میراب</span>
            </div>
        </div>
    );
}