// hooks/useGridData.ts
import { useCallback, useEffect, useRef, useState } from "react";
import { getGrid } from "@/lib/api/formGrid";
import type { GridResponse } from "@/types";

export function useGridData(formId: string | null) {
  const [grid, setGrid] = useState<GridResponse | null>(null);
  const [err, setErr] = useState<string | null>(null);
  const [isFetching, setIsFetching] = useState(false);
  const ctrlRef = useRef<AbortController | null>(null);

  const fetchGrid = useCallback(async () => {
    if (!formId) return;

    // cancel any in-flight request
    ctrlRef.current?.abort();
    const ctrl = new AbortController();
    ctrlRef.current = ctrl;

    setErr(null);
    setIsFetching(true);

    try {
      const data = await getGrid(formId, ctrl.signal);
      setGrid(data);
    } catch (e: any) {
      if (e?.name === "CanceledError" || e?.name === "AbortError") return;
      setErr(e?.message ?? "خطا در دریافت اطلاعات");
    } finally {
      setIsFetching(false);
    }
  }, [formId]);

  useEffect(() => {
    if (!formId) return;
    setGrid(null);
    fetchGrid();
    return () => ctrlRef.current?.abort();
  }, [formId, fetchGrid]);


  const refetch = useCallback(async () => {
    setGrid(null);
    await fetchGrid();
  }, [fetchGrid]);

  // still keeps your original "derived" loading
  const loading = (!!formId && !err && grid === null) || isFetching;

  return { grid, loading, err, setGrid, refetch };
}
