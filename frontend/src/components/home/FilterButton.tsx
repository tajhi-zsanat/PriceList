import {
  useState,
  useRef,
  useEffect,
  type JSX,
  type Dispatch,
  type SetStateAction,
} from "react";
import Spinner from "../Spinner";

type BasicItem = {
  id: number;
  name: string;
};

interface FilterButtonProps<TItem extends BasicItem = BasicItem> {
  data: TItem[];
  loading: boolean;
  isOpen: boolean;
  setIsOpen: Dispatch<SetStateAction<boolean>>;
  icon: string;
  label: string; // title above placeholder (used for accessibility/semantics)
  placeholder?: string;
  arrowIcon: string;
  selectedLabel: string | null;
  disabled?: boolean;
  onSelect: (id: number) => void;
}

export default function FilterButton<TItem extends BasicItem = BasicItem>({
  data,
  loading,
  isOpen,
  setIsOpen,
  icon,
  label,
  placeholder,
  arrowIcon,
  selectedLabel,
  disabled = false,
  onSelect,
}: FilterButtonProps<TItem>): JSX.Element {
  const dropdownRef = useRef<HTMLDivElement>(null);
  const [hasOpenedOnce, setHasOpenedOnce] = useState(false);

  useEffect(() => {
    function handleClick(e: MouseEvent) {
      if (
        dropdownRef.current &&
        !dropdownRef.current.contains(e.target as Node)
      ) {
        setIsOpen(false);
      }
    }

    document.addEventListener("mousedown", handleClick);
    return () => document.removeEventListener("mousedown", handleClick);
  }, [setIsOpen]);

  const handleToggle = () => {
    if (disabled) return;
    setIsOpen(!isOpen);
    if (!hasOpenedOnce) setHasOpenedOnce(true);
  };

  const showLabel = selectedLabel ?? placeholder ?? label;

  return (
    <div className="relative inline-block" ref={dropdownRef} dir="rtl">
      <button
        type="button"
        onClick={handleToggle}
        disabled={disabled}
        className={`
          flex items-center gap-2 bg-white text-[#636363]
          p-2 rounded-lg border border-[#CFD8DC]
          transition hover:bg-[#f2f5f7] cursor-pointer text-base justify-between
          ${disabled ? "opacity-60 cursor-not-allowed hover:bg-white" : ""}
        `}
        aria-label={label}
      >
        <img src={icon} alt="" aria-hidden="true" />

        {loading ? (
          <div className="flex-1 flex justify-center">
            <Spinner />
          </div>
        ) : (
          <span
            className={`flex-1 text-right text-sm ${selectedLabel ? "text-[#3F414D]" : "text-gray-400"
              }`}
          >
            {showLabel}
          </span>
        )}

        <img
          src={arrowIcon}
          className={`transition-transform duration-200 ${isOpen ? "rotate-180" : "rotate-0"
            }`}
          alt=""
          aria-hidden="true"
        />
      </button>

      {/* Dropdown Panel */}
      {isOpen && !loading && !disabled && (
        <ul
          className="
            absolute top-full mt-2 z-20 w-48 max-h-64 overflow-y-auto
            bg-white border border-[#CFD8DC] rounded-lg shadow-lg
          "
        >
          {data.length === 0 ? (
            <li className="p-3 text-center text-sm text-gray-500">
              {hasOpenedOnce ? "داده‌ای موجود نیست" : "در حال بارگذاری..."}
            </li>
          ) : (
            data.map((item) => (
              <li
                key={item.id}
                onClick={() => {
                  onSelect(item.id);
                  setIsOpen(false);
                }}
                className="
                  p-2 text-sm cursor-pointer text-[#3F414D]
                  hover:bg-[#f2f5f7] transition
                "
              >
                {item.name}
              </li>
            ))
          )}
        </ul>
      )}
    </div>
  );
}
