
import ActionBar from "./ActionBar";
import FilterBar from "./FilterBar";
import FormsTable from "./FormsTable";
import EmptyState from "./EmptyState";
import Loading from "./Loading";
import ErrorBanner from "./ErrorBanner";
import type { FormsContextType } from "@/types";
import { useOutletContext } from "react-router-dom";
import frameIcon from "@/assets/img/admin/ChangeFrame.png"
import { useEffect } from "react";

export default function Forms() {
  const { data, loading, error, reload } = useOutletContext<FormsContextType>();

  useEffect(() => {
    reload();
  }, [reload]);
  return (
    <section className="w-full mt-4" role="region" aria-label="فرم‌ها">

      <ActionBar />
      <div className="p-6 flex items-center justify-between bg-[#F5F5F5] rounded-[12px] mt-4">
        <FilterBar />
        <div>
          <img src={frameIcon} alt="" aria-hidden="true" />
        </div>
      </div>

      {loading && <Loading />}
      {error && <ErrorBanner message={error} />}

      {!loading && !error && (
        data.length === 0 ? <EmptyState text="هیچ دسته‌ای ثبت نشده است." /> : <FormsTable data={data} />
      )}
    </section>
  );
}
