import FarsiText from "@/components/FarsiText";
import more from "@/assets/img/more icon-mobile.png";
import type { FormListItemDto } from "@/types";
import { useNavigate } from "react-router-dom";

export default function Row({ item }: { item: FormListItemDto }) {
  const navigate = useNavigate();
  const categories = `${item.categoryName}`;
  const groups = `${item.groupName}`;

  const handleRowClick = () => {
    navigate(`/admin/products?formId=${item.id}`);
  };

  return (
    <div
      onClick={handleRowClick}
      className="
        grid grid-cols-13 items-center text-center
        rounded-[20px] py-5 cursor-pointer
        odd:bg-[#ECEFF1] even:bg-[#F5F5F5]
        hover:scale-[1.01]
         transition-scale duration-200 ease-in-out
      "
      role="row"
    >
      <span className="col-span-1">
        <input
          type="checkbox"
          className="size-4 align-middle cursor-pointer"
          onClick={(e) => e.stopPropagation()}
        />
      </span>

      <span className="col-span-1"><FarsiText>{item.id}</FarsiText></span>
      <span className="col-span-3">{categories}/{groups}</span>
      <span className="col-span-3">{item.brandName}</span>
      <span className="col-span-1">
        <FarsiText>{item.countProduct}</FarsiText>
      </span>
      <span className="col-span-3">
        <FarsiText>{item.updatedDate}</FarsiText>
      </span>

      <span
        className="col-span-1"
        onClick={(e) => {
          e.stopPropagation();
          console.log("open more menu or dialog...");
        }}
      >
        <img className="mx-auto cursor-pointer" src={more} alt="بیشتر" />
      </span>
    </div>
  );
}
