// src/components/home/HeroSearchBox.tsx
import type { JSX } from "react";
import FilterButton from "./FilterButton";

interface HeroSearchBoxProps {
  categoryIcon: string;
  arrowDownIcon: string;
}

export default function HeroSearchBox({
  categoryIcon,
  arrowDownIcon,
}: HeroSearchBoxProps): JSX.Element {
  return (
    <div
      className="
        absolute right-1/2 translate-x-1/2
        w-6xl -bottom-18 bg-white rounded-xl p-4
        border border-[#CFD8DC]
        shadow-[0px_0px_16px_0px_#D9D9D9]
      "
    >
      <p className="font-medium">محصول مورد نظر خود را انتخاب کنید</p>

      <div className="flex items-center justify-between mt-4">
        <div className="flex items-center gap-2">
          <FilterButton
            icon={categoryIcon}
            label="دسته بندی"
            arrowIcon={arrowDownIcon}
          />
          <FilterButton
            icon={categoryIcon}
            label="دسته بندی"
            arrowIcon={arrowDownIcon}
          />
          <FilterButton
            icon={categoryIcon}
            label="دسته بندی"
            arrowIcon={arrowDownIcon}
          />
        </div>

        <button className="bg-[#1F78AE] text-white py-2 md:px-16 rounded-[10px] cursor-pointer">
          جستجو
        </button>
      </div>
    </div>
  );
}
