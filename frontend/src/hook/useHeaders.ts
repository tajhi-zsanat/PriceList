import { useEffect, useRef, useState } from "react";
import api from "@/lib/api";
import type { ProductHeader } from "@/types";


export function useHeaders(brandId?: string, typeId?: string) {
const [headers, setHeaders] = useState<ProductHeader[]>([]);
const ctrlRef = useRef<AbortController | null>(null);


useEffect(() => {
ctrlRef.current?.abort();
const controller = new AbortController();
ctrlRef.current = controller;


(async () => {
try {
if (!brandId || !typeId) { setHeaders([]); return; }
const res = await api.get<ProductHeader[]>("/api/Header/by-categories", {
params: { brandId, typeId },
signal: controller.signal,
});
setHeaders(res.data ?? []);
} catch (e: any) {
if (e?.code === "ERR_CANCELED" || e?.name === "CanceledError") return;
console.warn("Failed to load headers:", e?.message ?? e);
} finally {
if (ctrlRef.current === controller) ctrlRef.current = null;
}
})();


return () => controller.abort();
}, [brandId, typeId]);


return headers;
}