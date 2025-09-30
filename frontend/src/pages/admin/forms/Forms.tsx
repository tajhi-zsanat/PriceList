import { useEffect, useState } from "react";
import type { FormListItemDto } from "@/types";
import api from "@/lib/api";
import ActionBar from "./ActionBar";
import FilterBar from "./FilterBar";
import FormsTable from "./FormsTable";
import EmptyState from "./EmptyState";
import Loading from "./Loading";
import ErrorBanner from "./ErrorBanner";

export default function Forms() {
  const [data, setData] = useState<FormListItemDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const controller = new AbortController();
    setLoading(true);
    setError(null);

    api.get<FormListItemDto[]>("/api/Form", { signal: controller.signal })
      .then(r => setData(r.data ?? []))
      .catch(err => {
        if (err?.code !== "ERR_CANCELED") {
          setError(err?.response?.data ?? err.message);
        }
      })
      .finally(() => setLoading(false));

    return () => controller.abort();
  }, []);

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
