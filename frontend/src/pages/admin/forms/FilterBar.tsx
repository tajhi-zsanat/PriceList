import arrangement from '@/assets/img/admin/solar_sort-linear.png';
import filter from '@/assets/img/admin/mage_filter.png';
import bottomArrow from '@/assets/img/admin/keyboard_arrow_down.png';

function SolidButton({
  icon, label,
}: { icon: string; label: string }) {
  return (
    <button
      type="button"
      className="flex items-center gap-2 bg-white text-[#636363] p-2 rounded-lg border border-[#636363] transition hover:bg-[#f2f5f7]"
    >
      <img src={icon} alt="" aria-hidden="true" className="" />
      <span>{label}</span>
    </button>
  );
}

function DropdownButton({ label }: { label: string }) {
  return (
    <button
      type="button"
      className="flex items-center gap-2 bg-white text-[#636363] p-2 rounded-lg border border-[#636363] transition hover:bg-[#f2f5f7]"
    >
      <span>{label}</span>
      <img src={bottomArrow} alt="" aria-hidden="true" className="" />
    </button>
  );
}

export default function FilterBar() {
  return (
    <div className="flex items-center gap-4">
      <SolidButton icon={arrangement} label="چیدمان" />
      <SolidButton icon={filter} label="فیلتر" />
      {["دسته بندی", "قیمت", "برند", "وضعیت تایید"].map(lbl => (
        <DropdownButton key={lbl} label={lbl} />
      ))}
    </div>
  );
}
