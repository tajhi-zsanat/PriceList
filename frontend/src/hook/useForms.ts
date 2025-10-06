// hooks/useForms.ts
import { useState, useEffect, useCallback } from "react";
import type { FormListItemDto } from "@/types";
import api from "@/lib/api";

export default function useForms() {
  const [data, setData] = useState<FormListItemDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchData = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const r = await api.get<FormListItemDto[]>("/api/Form");
      setData(r.data ?? []);
    } catch (err: any) {
      setError(err?.response?.data ?? err.message);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  return { data, loading, error, reload: fetchData };
}
