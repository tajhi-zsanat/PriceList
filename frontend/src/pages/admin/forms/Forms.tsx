
import ActionBar from "./ActionBar";
import FilterBar from "./FilterBar";
import FormsTable from "./FormsTable";
import EmptyState from "./EmptyState";
import Loading from "./Loading";
import ErrorBanner from "./ErrorBanner";
import type { FormsContextType } from "@/types";
import { useOutletContext } from "react-router-dom";

export default function Forms() {
  const { data, loading, error } = useOutletContext<FormsContextType>();

  return (
    <section className="w-full mt-4" role="region" aria-label="فرم‌ها">
      <ActionBar />
      <FilterBar />

      {loading && <Loading />}
      {error && <ErrorBanner message={error} />}

      {!loading && !error && (
        data.length === 0 ? <EmptyState text="هیچ دسته‌ای ثبت نشده است." /> : <FormsTable data={data} />
      )}
    </section>
  );
}
