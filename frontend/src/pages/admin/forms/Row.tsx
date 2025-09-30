import { Checkbox } from "@/components/ui/checkbox";
import FarsiText from "@/components/FarsiText";
import more from "@/assets/img/more icon-mobile.png";
import type { FormListItemDto } from "@/types";

export default function Row({ item, index }: { item: FormListItemDto; index: number }) {
  const categories = `${item.categoryName}/${item.groupdName}/${item.typeName}`;

  return (
    <div
      className="
        grid grid-cols-8 items-center text-center
        rounded-[20px] p-4 cursor-pointer
        odd:bg-[#ECEFF1] even:bg-[#F5F5F5]
      "
      role="row"
    >
      <span><Checkbox /></span>
      <span>{index + 1}</span>
      <span className="col-span-2">{categories}</span>
      <span>{item.brandName}</span>
      <span><FarsiText>{item.productCount}</FarsiText></span>
      <span><FarsiText>{item.updatedDate}</FarsiText></span>
      <span><img className="mx-auto" src={more} alt="بیشتر" /></span>
    </div>
  );
}
