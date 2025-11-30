import type { JSX } from "react";

interface FilterButtonProps {
  icon: string;
  label: string;
  arrowIcon: string;
}

export default function FilterButton({
  icon,
  label,
  arrowIcon,
}: FilterButtonProps): JSX.Element {
  return (
    <button
      type="button"
      className="flex items-center gap-2 bg-white text-[#636363] p-2 rounded-lg border border-[#CFD8DC] transition hover:bg-[#f2f5f7] cursor-pointer text-base"
    >
      <img src={icon} alt="" aria-hidden="true" />
      <span className="text-[#3F414D]">{label}</span>
      <img src={arrowIcon} alt="" aria-hidden="true" />
    </button>
  );
}
