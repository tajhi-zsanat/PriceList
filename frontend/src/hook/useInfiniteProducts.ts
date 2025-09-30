import { useCallback, useEffect, useRef, useState } from "react";
import api from "@/lib/api";
import type { FeatureBucketsResponse, UseInfiniteProductsArgs } from "@/types";

export function useInfiniteProducts({ params, take = 1 }: UseInfiniteProductsArgs) {
    const [data, setData] = useState<FeatureBucketsResponse | null>(null);
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
        const { categoryId, groupId, typeId, brandId } = params;
        if (loadingRef.current || !hasMoreRef.current) return;
        if (!categoryId || !groupId || !typeId || !brandId) return;


        cancel();
        const controller = new AbortController();
        ctrlRef.current = controller;


        loadingRef.current = true;
        setLoading(true);
        setError(null);


        try {
            const res = await api.get<FeatureBucketsResponse>("/api/Products/by-categories", {
                params: { categoryId, groupId, typeId, brandId, skip: nextSkip, take },
                signal: controller.signal,
            });


            const page = res.data;
            setData(prev => prev ? {
                ...page,
                items: [...prev.items, ...page.items],
                returnedCount: (prev.returnedCount ?? 0) + (page.returnedCount ?? page.items.length ?? 0),
                totalProductCount: page.totalProductCount ?? prev.totalProductCount,
            } : page);


            const batchCount = (page.returnedCount ?? page.items.length ?? 0);
            const newSkip = nextSkip + batchCount;
            setSkip(newSkip);


            const noMore = page.hasMore === false || batchCount === 0 || batchCount < take ||
                (typeof page.totalCount === "number" && newSkip >= page.totalCount);
            setHasMore(!noMore);
        } catch (e: any) {
            if (e?.code !== "ERR_CANCELED" && e?.name !== "CanceledError") {
                setError(e?.response?.data ?? e?.message ?? "Failed to load");
            }
        } finally {
            setLoading(false);
            loadingRef.current = false;
            if (ctrlRef.current === controller) ctrlRef.current = null;
        }
    }, [params.categoryId, params.groupId, params.typeId, params.brandId, take]);


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