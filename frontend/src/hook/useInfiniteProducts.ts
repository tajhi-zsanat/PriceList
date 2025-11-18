import { useCallback, useEffect, useRef, useState } from "react";
import api from "@/lib/api";
import type { FormCellsScrollResponseDto, GridGroup, UseInfiniteProductsArgs } from "@/types";

export function useInfiniteProducts({ params, take = 1 }: UseInfiniteProductsArgs) {
    const [data, setData] = useState<FormCellsScrollResponseDto | null>(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [skip, setSkip] = useState(0);
    const [hasMore, setHasMore] = useState(true);


    const ctrlRef = useRef<AbortController | null>(null);
    const loadingRef = useRef(false);
    const hasMoreRef = useRef(true);
    const skipRef = useRef(0);


    useEffect(() => { hasMoreRef.current = hasMore; }, [hasMore]);
    useEffect(() => { skipRef.current = skip; }, [skip]);


    const cancel = () => { ctrlRef.current?.abort(); ctrlRef.current = null; };


    const fetchPage = useCallback(async (nextSkip: number) => {
        const { categoryId, groupId, brandId } = params;
        if (loadingRef.current || !hasMoreRef.current) return;
        if (!categoryId || !groupId || !brandId) return;

        cancel();
        const controller = new AbortController();
        ctrlRef.current = controller;

        loadingRef.current = true;
        setLoading(true);
        setError(null);

        try {
            const res = await api.get<FormCellsScrollResponseDto>("/api/Product", {
                params: { categoryId, groupId, brandId, skip: nextSkip, take },
                signal: controller.signal,
            });

            const page = res.data;

            setData(prev => {
                if (!prev) return page;

                const groupsById = new Map<number, GridGroup>();
                for (const g of prev.cells) {
                    groupsById.set(g.featureId, { ...g, rows: [...g.rows] });
                }
                for (const g of page.cells) {
                    const existing = groupsById.get(g.featureId);
                    if (!existing) {
                        groupsById.set(g.featureId, g);
                    } else {
                        const existingRowIds = new Set(existing.rows.map(r => r.rowId));
                        const mergedRows = [
                            ...existing.rows,
                            ...g.rows.filter(r => !existingRowIds.has(r.rowId)),
                        ];
                        existing.rows = mergedRows;
                        existing.count = mergedRows.length;
                    }
                }
                const mergedCells = Array.from(groupsById.values());

                const pageRowCount = page.cells.reduce(
                    (sum, grp) => sum + grp.rows.length,
                    0
                );
                const prevRowCount = prev.cells.reduce(
                    (sum, grp) => sum + grp.rows.length,
                    0
                );

                return {
                    ...prev,
                    headers: prev.headers,
                    cells: mergedCells,
                    meta: {
                        ...prev.meta,
                        ...page.meta,
                        returnedCount: prevRowCount + pageRowCount,
                    },
                };
            });

            const pageRowCount = page.cells.reduce(
                (sum, grp) => sum + grp.rows.length,
                0
            );
            const newSkip = nextSkip + pageRowCount;
            setSkip(newSkip);

            setHasMore(page.meta.hasMore);
        } catch (e: any) {
            if (e?.code !== "ERR_CANCELED" && e?.name !== "CanceledError") {
                setError(e?.response?.data ?? e?.message ?? "Failed to load");
            }
        } finally {
            setLoading(false);
            loadingRef.current = false;
            if (ctrlRef.current === controller) ctrlRef.current = null;
        }
    }, [params.categoryId, params.groupId, params.brandId, take]);



    // reset & first load on param change
    useEffect(() => {
        setData(null);
        setSkip(0);
        setHasMore(true);
        hasMoreRef.current = true; skipRef.current = 0;
        fetchPage(0);
        return cancel;
    }, [fetchPage]);


    return { data, loading, error, hasMore, skip, loadMore: () => fetchPage(skipRef.current), cancel };
}