import FarsiText from "@/components/FarsiText";
import more from "@/assets/img/more icon-mobile.png";
import archives from "@/assets/img/admin/archives.png";
import type { FormListItemDto, FormsContextType } from "@/types";
import { useNavigate, useOutletContext } from "react-router-dom";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import { useState } from "react";
import { archivesForm } from "@/lib/api/formGrid";
import { toast } from "sonner";
import { cn } from "@/lib/utils";

export default function Row({ item }: { item: FormListItemDto }) {
  const { reload } = useOutletContext<FormsContextType>();
  const [busy, setBusy] = useState(false);
  const navigate = useNavigate();
  const categories = `${item.categoryName}`;
  const groups = `${item.groupName}`;

  const handleRowClick = () => {
    navigate(`/admin/products?formId=${item.id}`);
  };

  const removeForm = async (formId: number) => {
    if (busy) return;

    if (!formId) {
      toast.warning("شناسه نامعتبر می‌باشد.");
      return;
    }

    setBusy(true);
    try {
      const ctrlForm = new AbortController();
      await archivesForm({ formId }, ctrlForm.signal);
      toast.success("ردیف حذف گردید.");
      await reload();
    } catch {
      toast.error("حذف ناموفق بود.");
    } finally {
      setBusy(false);
    }
  };
  
  return (
    <div
      onClick={handleRowClick}
      className={cn(
        "grid grid-cols-13 items-center text-center rounded-[20px] py-5 odd:bg-[#ECEFF1] even:bg-[#F5F5F5] hover:bg-[#DDE2E4] transition-scale duration-200 ease-in-out",
        item.isdeleted &&  "opacity-50"
      )}

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

      <span className="col-span-1">
        <DropdownMenu
          modal={false} dir="rtl"
        >
          <DropdownMenuTrigger
            className="flex justify-center items-center m-auto cursor-pointer"
            onClick={(e) => e.stopPropagation()}
          >
            <img
              className="m-auto"
              src={more} alt="بیشتر"
            />
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end">
            <DropdownMenuItem
              className="focus-visible:hidden"
              onClick={(e) => {
                e.stopPropagation();
                removeForm(item.id)
              }}
            >
              <img
                className=""
                src={archives}
                alt="حذف"
                aria-hidden="true"
              />
              <span>{!item.isdeleted ? "بایگانی فرم" : "خارج نمودن از بایگانی"}</span>
            </DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      </span>
    </div>
  );
}
