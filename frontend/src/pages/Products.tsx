import { useEffect, useRef, useState } from "react";
import { useLocation, useParams } from "react-router-dom";
import Breadcrumbs from "../components/Breadcrumbs";
import "./Products.css";
import api from "../lib/api";
import type { PaginatedResult, ProductListItemDto } from "../types";
import time from '../assets/img/ion_time-outline.png';
import warning from '../assets/img/warning.png';
import pdf from '../assets/img/pdf.png';
import print from '../assets/img/print.png';
import search from '../assets/img/search.png';
import expand from '../assets/img/flowbite_expand-outline.png';

export default function ProductsBySupplier() {
  const { categoryId, groupId, typeId, brandId } = useParams();

  const loc = useLocation() as {
    state?: { categoryName?: string; groupName?: string; typeName?: string; brandName?: string };
  };
  const { categoryName, groupName, typeName, brandName } = loc.state || {};

  const [items, setItems] = useState<ProductListItemDto[]>([]);
  const [page, setPage] = useState(1);
  const pageSize = 12;

  const [totalCount, setTotalCount] = useState(0);
  const [hasMore, setHasMore] = useState(true);
  const [loadingInitial, setLoadingInitial] = useState(false);
  const [loadingMore, setLoadingMore] = useState(false);
  const [err, setErr] = useState<string | null>(null);

  const loaderRef = useRef<HTMLDivElement | null>(null);

  // Reset when route params change
  useEffect(() => {
    setItems([]);
    setPage(1);
    setHasMore(true);
    setTotalCount(0);
    setErr(null);
  }, [categoryId, groupId, typeId, brandId]);

  // Fetch page
  useEffect(() => {
    if (!categoryId || !groupId || !typeId || !brandId) return;
    if (!hasMore) return;

    const controller = new AbortController();

    // set the right loading flag
    if (page === 1) setLoadingInitial(true);
    else setLoadingMore(true);

    api.get<PaginatedResult<ProductListItemDto>>(
      "/api/Products/by-categories",
      {
        params: {
          brandId: Number(brandId),
          categoryId: Number(categoryId),
          groupId: Number(groupId),
          typeId: Number(typeId),
          page,
          pageSize,
        },
        signal: controller.signal,
      }
    )
      .then((res) => {
        const p = res.data;
        setItems((prev) => {
          // append with de-dup (by id)
          const merged = [...prev, ...p.items];
          const seen = new Set<number>();
          return merged.filter((x) => {
            const id = x.id;
            if (seen.has(id)) return false;
            seen.add(id);
            return true;
          });
        });
        setTotalCount(p.totalCount);
        setHasMore(p.hasNext);
      })
      .catch((e: any) => {
        if (e?.name !== "CanceledError") {
          setErr(e?.response?.data ?? e.message ?? "خطا در دریافت داده‌ها");
        }
      })
      .finally(() => {
        if (page === 1) setLoadingInitial(false);
        else setLoadingMore(false);
      });

    return () => controller.abort();
  }, [categoryId, groupId, typeId, brandId, page, hasMore]);

  // IntersectionObserver for infinite scroll
  useEffect(() => {
    const node = loaderRef.current;
    if (!node) return;

    const obs = new IntersectionObserver(
      (entries) => {
        const first = entries[0];
        // only load next page if not currently loading and still more
        const isLoading = loadingInitial || loadingMore;
        if (first.isIntersecting && !isLoading && hasMore) {
          setPage((p) => p + 1);
        }
      },
      {
        root: null,
        rootMargin: "300px", // start earlier for smoother loading
        threshold: 0,
      }
    );

    obs.observe(node);
    return () => obs.disconnect();
  }, [loadingInitial, loadingMore, hasMore]);

  // Simple skeletons for initial load (optional)
  const renderSkeletons = (count = pageSize) => (
    <ul className="product-grid">
      {Array.from({ length: count }).map((_, i) => (
        <li key={`s-${i}`} className="product-card skeleton">
          <div className="skeleton-img" />
          <div className="skeleton-line w-3/4" />
          <div className="skeleton-line w-1/2" />
          <div className="skeleton-line w-1/3" />
        </li>
      ))}
    </ul>
  );

  return (
    <div className="px-16 pt-8">
      <Breadcrumbs />

      <div className="border border-[#CFD8DC] rounded-xl">

        {/* Error */}
        {err && <div className="text-red-600 mb-3">خطا: {err}</div>}

        <div className="">
          <div className="p-8">
            <p className="mb-2 text-xl font-[400]">
              لیست قیمت محصولات {brandName} <span className="text-sm text-[#636363]">({totalCount} محصول)</span>
            </p>

            <div className="flex items-center gap-4 mb-8">
              <div className="flex items-center gap-2">
                <img src={time} alt="time" />
                <p className="text-[#636363]">آخرین بروز رسانی 22 تیر 1404</p>
              </div>
              <div className="bg-[#636363] w-[1px] h-4"></div>
              <div className="flex items-center gap-2">
                <img src={warning} alt="time" />
                <p className="text-[#636363]">کد تامین کننده : <span>45</span></p>
              </div>
            </div>

            <div className="flex items-center gap-4 mb-4">
              <button className="button-outline">
                <img src={pdf} alt="pdf" />
                <span>خروجی PDF</span>
              </button>
              <button className="button-outline">
                <img src={print} alt="print" />
                <span>چاپ PDF</span>
              </button>
            </div>
          </div>

          <div className="flex items-center justify-between bg-[#ECEFF1] p-4">
            <ul className="flex items-center gap-3">
              <li className="bg-white rounded-lg py-2 px-8">
                زانو
              </li>
              <li className="bg-white rounded-lg py-2 px-8">
                سه راهی
              </li>
              <li className="bg-white rounded-lg py-2 px-8">
                لوله
              </li>
            </ul>
            <div className="flex items-center gap-3">
              <div className="relative w-64">
                <input
                  type="text"
                  placeholder="جستجو"
                  className="w-full border border-gray-300 rounded-lg py-2 pr-10 pl-3 focus:outline-none focus:border-blue-500"
                />
                <img
                  src={search}
                  alt="search"
                  className="absolute right-3 top-1/2 -translate-y-1/2 w-5 h-5 text-gray-400 pointer-events-none"
                />
              </div>
              <div>
                <img src={expand} alt="expand" />
              </div>
            </div>

          </div>

          {/* Initial loading skeleton */}
          {loadingInitial && renderSkeletons(8)}

          {/* Empty (no error, no loading, no items) */}
          {!loadingInitial && items.length === 0 && !err && (
            <div className="text-gray-600">موردی یافت نشد.</div>
          )}

          {/* Items */}
          {!loadingInitial && items.length > 0 && (
            <div className="overflow-x-auto rounded-lg border border-gray-200 shadow-sm mx-8">
              <table className="min-w-full divide-y divide-gray-200 text-sm text-right">
                <thead className="bg-gray-100 text-gray-700 font-medium">
                  <tr>
                    <th className="px-4 py-2">ردیف</th>
                    <th className="px-4 py-2">عکس</th>
                    <th className="px-4 py-2">فایل</th>
                    <th className="px-4 py-2">شرح کالا</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-gray-100">
                  {items.map((p, i) => {
                    const cover = p.images?.[0];
                    return (
                      <tr key={p.id} className="hover:bg-gray-50">
                        <td className="px-4 py-2">{i + 1}</td>
                        <td className="px-4 py-2">
                          {cover ? (
                            <img
                              src={cover}
                              alt={p.model ?? "product"}
                              loading="lazy"
                              decoding="async"
                              className="h-12 w-12 object-cover rounded-md border"
                            />
                          ) : (
                            <span className="text-gray-400">بدون تصویر</span>
                          )}
                        </td>
                        <td className="px-4 py-2">{p.model ?? "—"}</td>
                        <td className="px-4 py-2">{p.description ?? "—"}</td>
                        <td className="px-4 py-2">
                          {p.price != null ? p.price.toLocaleString() : "—"} ریال
                        </td>
                        <td className="px-4 py-2">
                          {p.customProperties?.length > 0 ? (
                            <ul className="list-disc pr-5 space-y-1 text-gray-600">
                              {p.customProperties.slice(0, 3).map((cp, idx) => (
                                <li key={idx}>
                                  <span className="font-medium">{cp.key}:</span>{" "}
                                  {cp.value}
                                </li>
                              ))}
                            </ul>
                          ) : (
                            <span className="text-gray-400">—</span>
                          )}
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
          )}

        </div>

        {/* Sentinel area */}
        <div ref={loaderRef} className="sentinel">
          {loadingMore && <div className="spinner" aria-label="Loading more…" />}
          {!hasMore && items.length > 0 && (
            <div className="end-of-list">همه‌ی موارد بارگذاری شد</div>
          )}
        </div>
      </div>
    </div>
  );
}
