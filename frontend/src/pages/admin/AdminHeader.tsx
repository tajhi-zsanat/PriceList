import { Link } from "react-router-dom";
import arrow from "@/assets/img/admin/arrow_forward.png"
import logo from "@/assets/img/pikatak.png";
import SearchInput from "@/components/products/SearchInput";
import bell from "@/assets/img/bell.png";
import avator from "@/assets/img/admin/Avatar-Placeholder.png";
import plus from "@/assets/img/admin/tabler_plus.png";
import CreateFormModal from "@/components/admin/CreateFormModal";
import { useState } from "react";
import type { AdminHeaderProps } from "@/types";

export default function AdminHeader({ onFormCreated }: AdminHeaderProps) {
    const [modalOpen, setModalOpen] = useState(false);

    return (
        <header>
            <Link className="flex justify-end items-center gap-1 text-[#1F78AE] underline decoration-[#1F78AE]" to='/'>
                <span className="font-medium">بازگشت به پیکاتک</span>
                <img src={arrow} alt="بازگشت" />
            </Link>
            <div className="bg-[#F5F5F5] rounded-[12px] p-4">
                <div className="flex justify-between items-center">
                    <div className="flex items-center gap-8">
                        <img src={logo} alt="لوگو" />
                        <SearchInput
                            value={""}
                            onChange={() => { }}
                            className="w-96"
                        />
                    </div>
                    <div className="flex items-center">
                        <div className="flex justify-center items-center rounded-[10px] w-10 h-10 bg-[#CFE2FF]">
                            <img src={bell} alt="زنگ" />
                        </div>
                        <div className="flex">
                            <div className="flex justify-center items-center bg-[#CFE2FF] rounded-tr-[10px] rounded-br-[10px] pr-2.5 pl-5 -translate-x-2.5
              ">
                                <span className="text-[#1B1B1E] font-medium">محمد چراغ زاده</span>
                            </div>
                            <div className="flex justify-center items-center rounded-[10px] w-10 h-10 bg-[#CFE2FF] border border-[#1F78AE] z-10
              ">
                                <img src={avator} alt="کاربر" />
                            </div>
                        </div>
                    </div>
                </div>
                <div className="flex justify-between items-center mt-4">
                    <ul className="flex items-center gap-12 text-[#1B1B1E] mt-4">
                        <li className="cursor-pointer text-base">پیشخوان</li>
                        <li className="text-[#1F78AE] cursor-pointer text-base">فرم‌های من</li>
                        <li className="cursor-pointer text-base">تنظیمات</li>
                        <li className="cursor-pointer text-base">تعرفه‌ها</li>
                    </ul>
                    <CreateFormModal
                        open={modalOpen}
                        onOpenChange={setModalOpen}
                        onCreated={onFormCreated}
                        trigger={
                            <button className="button-outline">
                                <img src={plus} alt="plus" />
                                <span>افزودن فرم جدید</span>
                            </button>
                        }
                    />
                </div>
            </div>
        </header>
    );
}