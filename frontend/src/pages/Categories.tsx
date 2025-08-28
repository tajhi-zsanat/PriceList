import { useEffect, useState } from "react";
import api from "../lib/api";
import type { CategoryListItemDto } from "../types";

export default function Categories() {
  const [data, setData] = useState<CategoryListItemDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

useEffect(() => {
  const toArray = (x: any): any[] => {
    if (Array.isArray(x)) return x;
    if (x?.$values && Array.isArray(x.$values)) return x.$values; // ReferenceHandler.Preserve case
    if (x?.value && Array.isArray(x.value)) return x.value;       // OData / wrapper case
    return [];                                                     // unknown shape
  };

  api.get("/api/Category")
    .then(r => {
      console.log("API response:", r.data);
      const arr = toArray(r.data);
      if (arr.length === 0 && !Array.isArray(r.data)) {
        // show a quick error on unexpected shape
        setError("Unexpected response shape from /api/Category");
      }
      setData(arr);
    })
    .catch(e => {
      console.error("API error:", e);
      setError(e?.response?.data ?? e.message);
    })
    .finally(() => setLoading(false));
}, []);

  if (loading) return <div className="p-6">در حال بارگذاری…</div>;
  if (error)   return <div className="p-6 text-red-600">خطا: {error}</div>;

  return (
    <div className="p-6 space-y-4">
      <h1 className="text-2xl font-bold">دسته‌بندی‌ها</h1>
      <ul className="grid sm:grid-cols-2 lg:grid-cols-3 gap-4">
        {data.map(c => (
          <li key={c.id} className="bg-white p-4 rounded-2xl shadow">
            <div className="text-lg font-semibold">{c.name}</div>
            {c.imagePath && (
              <img className="mt-2 rounded-xl"
                   src={`${import.meta.env.VITE_API_BASE_URL}${c.imagePath}`}
                   alt={c.name} />
            )}
          </li>
        ))}
      </ul>
    </div>
  );
}
