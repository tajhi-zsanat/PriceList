import FarsiText from "@/components/FarsiText";
import moreIcon from "../../assets/img/more icon-mobile.png";
import productImgTest from "../../assets/img/product-test.png";
import pdfProduct from "../../assets/img/pdfProduct.png";
import type { ProductHeader } from "@/types";


type Product = {
    id: number | string;
    description?: string | null;
    price?: number | null;
    productHeaders?: { id: number; value: string }[];
};


type Props = { index: number; product: Product; headers: ProductHeader[] };
export default function ProductRow({ index, product, headers }: Props) {
    const phMap = new Map((product.productHeaders ?? []).map(ph => [ph.id, ph.value]));
    return (
        <tr>
            <td className="border border-gray-300 px-2 py-10 text-center">{index}</td>
            <td className="border border-gray-300 px-2 ">
                <span className="text-[#1F78AE] underline decoration-[#1F78AE]">2665</span>
                <img className="m-auto" src={productImgTest} alt="عکس محصول" />
            </td>
            <td className="border border-gray-300 px-2 py-10 "><img className="m-auto" src={pdfProduct} alt="فایل محصول" /></td>
            <td className="border border-gray-300 px-2 py-10 text-center">{product.description ?? "--"}</td>
            {headers.map(h => (
                <td key={`${product.id}-h-${h.id}`} className="border border-gray-300 px-2 py-10 text-center">{phMap.get(h.id) ?? "--"}</td>
            ))}
            <td className="border border-gray-300 px-2 py-10 text-center">عدد</td>
            <td className="border border-gray-300 px-2 py-10 text-center text-[#2A7906] font-bold">
                {product.price != null ? <FarsiText>{product.price.toLocaleString()} تومان</FarsiText> : "--"}
            </td>
            <td className="border border-gray-300 "><img className="m-auto" src={moreIcon} alt="بیشتر" /></td>
        </tr>
    );
}