import { Fragment, useCallback, useEffect, useRef, useState } from "react";
import { useLocation } from "react-router-dom";
import Breadcrumbs from "../../components/Breadcrumbs";
import "./Products.css";
import api from "../../lib/api";
import time from '../../assets/img/ion_time-outline.png';
import warning from '../../assets/img/warning.png';
import pdf from '../../assets/img/pdf.png';
import print from '../../assets/img/print.png';
import bell from '../../assets/img/bell.png';
import search from '../../assets/img/search.png';
import expand from '../../assets/img/flowbite_expand-outline.png';
import moreIcon from '../../assets/img/more icon-mobile.png';
import productImgTest from '../../assets/img/product-test.png';
import pdfProduct from '../../assets/img/pdfProduct.png';
import FarsiText from "@/components/FarsiText";
import { Switch } from "@/components/ui/switch";
import type { FeatureBucketsResponse, ProductHeader } from "@/types";

const TAKE = 1;

export default function ProductsBySupplier() {
  const [data, setData] = useState<FeatureBucketsResponse | null>(null);
  const [headers, setHeaders] = useState<ProductHeader[]>([]);
  const [loading, setLoading] = useState(false);
  const [skip, setSkip] = useState(0);
  const [hasMore, setHasMore] = useState(true);
  const [err, setErr] = useState<string | null>(null);

  const sentinelRef = useRef<HTMLDivElement | null>(null);
  const loadingRef = useRef(false);
  const hasMoreRef = useRef(true);       // <- read latest hasMore without re-creating callbacks
  const skipRef = useRef(0);             // <- same for skip

  useEffect(() => { hasMoreRef.current = hasMore; }, [hasMore]);
  useEffect(() => { skipRef.current = skip; }, [skip]);

  const loc = useLocation() as {
    state?: { categoryName?: string; groupName?: string; typeName?: string; brandName?: string };
  };
  const { brandName } = loc.state || {};

  // Stable fetcher (no dependencies)
  const fetchPage = useCallback(async (nextSkip: number, takeSize: number) => {
    if (loadingRef.current || !hasMoreRef.current) return;

    loadingRef.current = true;
    setLoading(true);
    setErr(null);

    try {
      const res = await api.get<FeatureBucketsResponse>("/api/Products/by-categories", {
        params: { brandId: 2, categoryId: 1, groupId: 1, typeId: 1, skip: nextSkip, take: takeSize },
      });

      const page = res.data;

      setData(prev => {
        if (!prev) return page;
        return {
          ...page,
          items: [...prev.items, ...page.items],
          returnedCount: (prev.returnedCount ?? 0) + (page.returnedCount ?? page.items.length ?? 0),
          totalProductCount: page.totalProductCount ?? prev.totalProductCount,
        };
      });

      // Safer: use the caller's skip + actual batch size
      const batchCount = (page.returnedCount ?? page.items.length ?? 0);
      const newSkip = nextSkip + batchCount;
      setSkip(newSkip);

      // Robust stop conditions
      const noMore =
        page.hasMore === false ||
        batchCount === 0 ||
        batchCount < takeSize ||                                        // <- stop when last page is smaller than take
        (typeof page.totalCount === "number" && newSkip >= page.totalCount);

      setHasMore(!noMore);
    } catch (e: any) {
      setErr(e?.response?.data ?? e?.message ?? "Failed to load");
    } finally {
      setLoading(false);
      loadingRef.current = false;
    }
  }, []);

  // Fetch headers once (not critical)
  useEffect(() => {
    (async () => {
      try {
        const res = await api.get<ProductHeader[]>("/api/Header/by-categories", {
          params: { brandId: 2, typeId: 1 },
        });
        setHeaders(res.data ?? []);
      } catch (e: any) {
        console.warn("Failed to load headers:", e?.message ?? e);
      }
    })();
  }, []);

  // First page: run ONCE on mount
  useEffect(() => {
    setData(null);
    setSkip(0);
    setHasMore(true);
    hasMoreRef.current = true;
    skipRef.current = 0;
    fetchPage(0, TAKE);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []); // <- important: no fetchPage in deps

  // IntersectionObserver
  useEffect(() => {
    const node = sentinelRef.current;
    if (!node) return;

    const observer = new IntersectionObserver(
      (entries) => {
        const first = entries[0];
        if (first.isIntersecting && hasMoreRef.current && !loadingRef.current) {
          fetchPage(skipRef.current, TAKE);
        }
      },
      { root: null, rootMargin: "100px", threshold: 0.01 }
    );

    observer.observe(node);
    return () => observer.unobserve(node);
  }, [fetchPage]); // stable

  let rowNo = 0;

  return (
    <div className="flex-1">
      <Breadcrumbs />
      <div className="px-16 pt-4 bg-white">
        <div className="border border-[#CFD8DC] rounded-xl">
          <div className="">
            <div className="p-8">
              <p className="mb-2 text-xl font-[400]">
                لیست قیمت محصولات {brandName}{" "}
                {data && (
                  <span className="text-sm text-[#636363]">
                    (<FarsiText>{data.totalProductCount}</FarsiText> محصول)
                  </span>
                )}
              </p>

              <div className="flex items-center gap-4 mb-8">
                <div className="flex items-center gap-2">
                  <img src={time} alt="time" />
                  <p className="text-[#636363]"><FarsiText>آخرین به روز رسانی 23 تیر 1404</FarsiText></p>
                </div>
                <div className="bg-[#636363] w-[1px] h-4" />
                <div className="flex items-center gap-2">
                  <img src={warning} alt="warning" />
                  <p className="text-[#636363]">کد تامین کننده : <FarsiText>45</FarsiText></p>
                </div>
              </div>

              <div className="flex justify-between items-center">
                <div className="flex items-center gap-4">
                  <button className="button-outline">
                    <img src={pdf} alt="pdf" />
                    <span>خروجی PDF</span>
                  </button>
                  <button className="button-outline">
                    <img src={print} alt="print" />
                    <span>چاپ PDF</span>
                  </button>
                  <button className="button-outline">
                    <img src={bell} alt="bell" />
                    <span>خبرم کن</span>
                  </button>
                </div>
                <div className="flex justify-between items-center gap-2">
                  <Switch className="[direction:ltr] data-[state=checked]:bg-[#1F78AE] data-[state=unchecked]:bg-gray-300" />
                  <span>ارزش افزوده</span>
                </div>
              </div>
            </div>

            <div className="flex items-center justify-between bg-[#ECEFF1] py-4 px-8">
              {data && data.items.length > 1 && (
                <ul className="flex items-center gap-3">
                  {data.items.map(f => (
                    <li key={f.featuresIDs} className="bg-white rounded-lg py-2 px-8 cursor-pointer">
                      {f.title}
                    </li>
                  ))}
                </ul>
              )}
              <div className="flex items-center justify-between gap-3">
                <div className="relative w-64">
                  <input
                    type="text"
                    placeholder="جستجو"
                    className="w-full bg-white border border-gray-300 rounded-lg py-2 pr-10 pl-3 focus:outline-none focus:border-blue-500"
                  />
                  <img
                    src={search}
                    alt="search"
                    className="absolute right-3 top-1/2 -translate-y-1/2 w-5 h-5 pointer-events-none"
                  />
                </div>
                <div>
                  <img src={expand} alt="expand" />
                </div>
              </div>
            </div>

            {data?.items?.length ? (
              <div className="overflow-x-auto mx-8 mb-4">
                <table className="min-w-full border border-[#CFD8DC] text-right border-collapse">
                  <thead className="bg-[#CFE2FF] text-gray-700 font-medium">
                    <tr>
                      <th className="border border-[#1F78AE] px-2 py-6 text-center w-16">ردیف</th>
                      <th className="border border-[#1F78AE] px-2 py-6 text-center">عکس</th>
                      <th className="border border-[#1F78AE] px-2 py-6 text-center">فایل</th>
                      <th className="border border-[#1F78AE] px-2 py-6 text-center">شرح کالا</th>
                      {headers?.map(h => (
                        <th key={h.id} className="border border-[#1F78AE] px-2 py-6 text-center">{h.value}</th>
                      ))}
                      <th className="border border-[#1F78AE] px-2 py-6 text-center">واحد</th>
                      <th className="border border-[#1F78AE] px-2 py-6 text-center">قیمت</th>
                      <th className="border border-[#1F78AE] text-center w-8" />
                    </tr>
                  </thead>

                  <tbody>
                    {data.items.map(item => (
                      <Fragment key={item.featuresIDs}>
                        <tr style={{ background: item.featureColor == null ? '#1F78AE' : item.featureColor }}>
                          <td className="text-white border border-gray-300 py-2 px-3" colSpan={7 + (headers?.length ?? 0)}>
                            {item.title}
                          </td>
                        </tr>

                        {item.products.map(product => {
                          rowNo += 1;
                          const phMap = new Map((product.productHeaders ?? []).map(ph => [ph.id, ph.value]));
                          return (
                            <tr key={product.id}>
                              <td className="border border-gray-300 px-2 py-10 text-center">{rowNo}</td>
                              <td className="border border-gray-300 px-2 ">
                                <span className="text-[#1F78AE] underline decoration-[#1F78AE]">2665</span>
                                <img className="m-auto" src={productImgTest} alt="عکس محصول" />
                              </td>
                              <td className="border border-gray-300 px-2 py-10 ">
                                <img className="m-auto" src={pdfProduct} alt="فایل محصول" />
                              </td>
                              <td className="border border-gray-300 px-2 py-10 text-center">{product.description ?? "--"}</td>

                              {headers.map(h => (
                                <td key={`${product.id}-h-${h.id}`} className="border border-gray-300 px-2 py-10 text-center">
                                  {phMap.get(h.id) ?? "--"}
                                </td>
                              ))}

                              <td className="border border-gray-300 px-2 py-10 text-center">عدد</td>
                              <td className="border border-gray-300 px-2 py-10 text-center text-[#2A7906] font-bold">
                                {product.price != null
                                  ? <FarsiText>{product.price.toLocaleString()} تومان</FarsiText>
                                  : "--"}
                              </td>
                              <td className="border border-gray-300 ">
                                <img className="m-auto" src={moreIcon} alt="بیشتر" />
                              </td>
                            </tr>
                          );
                        })}
                      </Fragment>
                    ))}
                  </tbody>
                </table>

                <div className="flex flex-col gap-2 border border-[#CFD8DC] border-t-0 rounded-b-[8px] p-3">
                  <span className="text-[#636363]">توضیحات</span>
                  <span>فرم گیج فشار میراب</span>
                </div>
              </div>
            ) : (
              !loading && <div className="p-8 text-center text-[#636363]">محصولی یافت نشد.</div>
            )}
          </div>

          {err && <div className="text-red-600 px-8 py-4">خطا در دریافت داده‌ها: {err}</div>}
          {loading && <div className="text-center text-sm opacity-70 py-4">در حال بارگذاری…</div>}
          {!hasMore && !loading && <div className="text-center text-sm opacity-70 py-4">همه موارد بارگذاری شد.</div>}

          <div ref={sentinelRef} aria-hidden="true" />
        </div>
      </div>
    </div>
  );
}
