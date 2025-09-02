import { useEffect, useMemo, useRef, useState } from "react";
import { useLocation, useParams } from "react-router-dom";
import api from "../lib/api";
import { imgUrl } from "../lib/helpers";
import Breadcrumbs from "../components/Breadcrumbs";
import type { LocState, SupplierSection, SupplierSummaryDto, SupplierProductsPageDto } from "../types";

export default function ProductsBySupplier() {
  const { categoryId, groupId, typeId, brandId } = useParams();
  const loc = useLocation() as { state?: LocState };
  const { brandName } = loc.state || {};

  const [loadingList, setLoadingList] = useState(false);
  const [listError, setListError] = useState<string | null>(null);

  const [suppliers, setSuppliers] = useState<SupplierSummaryDto[]>([]);
  const [supplierIndex, setSupplierIndex] = useState(0);

  // per-supplier sections that we render on the page
  const [sections, setSections] = useState<SupplierSection[]>([]);

  // prevent duplicate loads
  const [loadingPage, setLoadingPage] = useState(false);

  const pageSize = 20;

  // A single sentinel at the very bottom triggers loading next page or next supplier
  const sentinelRef = useRef<HTMLDivElement | null>(null);

  const filtersOk = useMemo(
    () => !!(categoryId && groupId && typeId && brandId),
    [categoryId, groupId, typeId, brandId]
  );

  // 1) Load ranked suppliers
  useEffect(() => {
    if (!filtersOk) return;

    setLoadingList(true);
    setListError(null);
    setSuppliers([]);
    setSections([]);
    setSupplierIndex(0);

    api
      .get<SupplierSummaryDto[]>("/api/Products/by-categories/suppliers-summary", {
        params: { brandId, categoryId, groupId, typeId, skip: 0, take: 20 }, // take as many as you want
      })
      .then((r) => setSuppliers(r.data))
      .catch((e) => setListError(e?.response?.data ?? e.message))
      .finally(() => setLoadingList(false));
  }, [filtersOk, brandId, categoryId, groupId, typeId]);

  // 2) Ensure the current supplier's first page is loaded
  useEffect(() => {
    debugger;

    if (!filtersOk) return;
    if (loadingPage) return;
    if (!suppliers.length) return;

    const current = suppliers[supplierIndex];
    if (!current) return; // no more suppliers

    // If we already created a section for this supplier, do nothing here.
    const already = sections.find((s) => s.supplierId === current.supplierId);
    if (already) return;

    // Load page 1 for this supplier
    setLoadingPage(true);
    api
      .get<SupplierProductsPageDto>("/api/Products/by-categories/by-supplier", {
        params: {
          brandId,
          categoryId,
          groupId,
          typeId,
          supplierId: current.supplierId,
          page: 1,
          pageSize,
        },
      })
      .then((r) => {
        const d = r.data;
        setSections((prev) => [
          ...prev,
          {
            supplierId: d.supplierId,
            supplierName: d.supplierName,
            items: d.items,
            page: d.page,
            hasNext: d.hasNext,
            totalCount: d.totalCount,
          },
        ]);
      })
      .catch((e) => setListError(e?.response?.data ?? e.message))
      .finally(() => setLoadingPage(false));
  }, [filtersOk, suppliers, supplierIndex, sections, loadingPage, brandId, categoryId, groupId, typeId]);

  // 3) IntersectionObserver to auto-load more for current supplier, or move to next supplier
  useEffect(() => {
    if (!filtersOk) return;
    if (!sentinelRef.current) return;

    const el = sentinelRef.current;
    const obs = new IntersectionObserver((entries) => {
      const entry = entries[0];
      if (!entry.isIntersecting) return;

      // Who is the current supplier?
      const sup = suppliers[supplierIndex];
      if (!sup) return; // no more suppliers

      // Find its section
      const sec = sections.find((s) => s.supplierId === sup.supplierId);
      if (!sec) return; // not yet created

      // If current supplier has more pages, load the next one
      if (sec.hasNext && !loadingPage) {
        setLoadingPage(true);
        api
          .get<SupplierProductsPageDto>("/api/Products/by-categories/by-supplier", {
            params: {
              brandId,
              categoryId,
              groupId,
              typeId,
              supplierId: sec.supplierId,
              page: sec.page + 1,
              pageSize,
            },
          })
          .then((r) => {
            const d = r.data;
            setSections((prev) =>
              prev.map((x) =>
                x.supplierId === sec.supplierId
                  ? {
                    ...x,
                    items: [...x.items, ...d.items],
                    page: d.page,
                    hasNext: d.hasNext,
                    totalCount: d.totalCount,
                  }
                  : x
              )
            );
          })
          .catch((e) => setListError(e?.response?.data ?? e.message))
          .finally(() => setLoadingPage(false));
        return;
      }

      // If no more pages for current supplier, move to next supplier
      if (!sec.hasNext) {
        // Only advance if next supplier exists
        if (supplierIndex + 1 < suppliers.length) {
          setSupplierIndex((i) => i + 1);
        }
      }
    }, { rootMargin: "400px 0px 400px 0px" }); // prefetch earlier

    obs.observe(el);
    return () => obs.disconnect();
  }, [
    filtersOk,
    suppliers,
    supplierIndex,
    sections,
    loadingPage,
    brandId,
    categoryId,
    groupId,
    typeId,
  ]);

  return (
    <div className="p-6 space-y-4">
      <Breadcrumbs />
      <h1 className="text-2xl font-bold">
        محصولات {brandName ? `«${brandName}»` : `برند ${brandId}`}
      </h1>

      {loadingList && <div>در حال بارگذاری تأمین‌کنندگان…</div>}
      {listError && <div className="text-red-600">خطا: {listError}</div>}

      {/* Sections per supplier */}
      {!loadingList && !listError && sections.length === 0 && (
        <div className="text-gray-500">موردی یافت نشد.</div>
      )}

      {sections.map((sec) => (
        <section key={sec.supplierId} className="space-y-3">
          <div className="flex items-baseline gap-2">
            <h2 className="text-xl font-semibold">{sec.supplierName}</h2>
            <span className="text-sm text-gray-500">
              {sec.totalCount.toLocaleString("fa-IR")} کالا
            </span>
          </div>

          <div className="grid sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
            {sec.items.map((p) => (
              <article key={p.id} className="bg-white p-4 rounded-2xl shadow">
                <div className="text-xs text-gray-500 mb-1">کد: {p.id}</div>
                <div className="font-semibold">{p.model ?? "—"}</div>
                <div className="text-sm text-gray-600 line-clamp-2 mt-1">
                  {p.description ?? ""}
                </div>
                {p.imagePath && (
                  <img
                    src={imgUrl(p.imagePath)}
                    alt={p.model ?? String(p.id)}
                    className="mt-2 rounded-xl w-full h-40 object-contain bg-gray-50"
                    loading="lazy"
                    decoding="async"
                    draggable={false}
                  />
                )}
                <div className="mt-3 font-bold">
                  {p.price != null
                    ? p.price.toLocaleString("fa-IR") + " تومان"
                    : "—"}
                </div>
              </article>
            ))}
          </div>
        </section>
      ))}

      {/* Global sentinel at the very bottom */}
      <div ref={sentinelRef} className="h-10" />

      {/* Small loading indicator while fetching next page/next supplier */}
      {loadingPage && (
        <div className="text-center text-sm text-gray-500">در حال بارگذاری…</div>
      )}
    </div>
  );
}
