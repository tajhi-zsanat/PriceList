import { useLocation, useParams } from "react-router-dom";
import Breadcrumbs from "@/components/Breadcrumbs";
import "./Products.css";
import time from '@/assets/img/ion_time-outline.png';
import warning from '@/assets/img/warning.png';
import expand from '@/assets/img/flowbite_expand-outline.png';
import FarsiText from "@/components/FarsiText";
import FeatureChips from "@/components/products/FeatureChips";
import PriceToolbar from "@/components/products/PriceToolbar";
import SearchInput from "@/components/products/SearchInput";
import InfiniteSentinel from "@/components/products/InfiniteSentinel";
import { useInfiniteProducts } from "@/hook/useInfiniteProducts";
import ProductHeader from "@/components/products/ProductHeader";

const TAKE = 10;

export default function ProductsBySupplier() {
    const { categoryId, groupId, typeId, brandId } = useParams();
    const loc = useLocation() as { state?: { brandName?: string } };
    const { brandName } = loc.state || {};

    const { data, loading, error, hasMore, loadMore } = useInfiniteProducts({ params: { categoryId, groupId, typeId, brandId }, take: TAKE });

    return (
        <div className="flex-1">
            <Breadcrumbs />
            <div className="px-16 pt-4 bg-white">
                <div className="border border-[#CFD8DC] rounded-xl">
                    <div className="p-8">
                        <p className="mb-2 text-xl font-[400]">
                            لیست قیمت محصولات {brandName}{" "}
                            {data && (
                                <span className="text-sm text-[#636363]">
                                    (<FarsiText>{data.meta.totalRows}</FarsiText> محصول)
                                </span>
                            )}
                        </p>
                        <div className="flex items-center gap-4 mb-8">
                            <div className="flex items-center gap-2"><img src={time} alt="time" />
                                <p className="text-[#636363]">
                                    تاریخ آخرین بروزرسانی:
                                    <FarsiText> {data?.meta.lastUpdate}</FarsiText>
                                </p>
                            </div>
                            <div className="bg-[#636363] w-[1px] h-4" />
                            <div className="flex items-center gap-2"><img src={warning} alt="warning" /><p className="text-[#636363]">کد تامین کننده : <FarsiText>45</FarsiText></p></div>
                        </div>
                        <PriceToolbar />
                    </div>

                    <div className="flex items-center justify-between bg-[#ECEFF1] py-4 px-8">
                        <FeatureChips data={data ?? null} />
                        <div className="flex items-center gap-3">
                            <SearchInput value={""} onChange={() => { }} />
                            <div><img src={expand} alt="expand" /></div>
                        </div>
                    </div>


                    {data?.cells?.length ? (
                        <ProductHeader formHeaders={data.headers} groupCells={data.cells} formName={data.meta.formName} />
                    ) : (!loading && <div className="p-8 text-center text-[#636363]">محصولی یافت نشد.</div>)}


                    {error && <div className="text-red-600 px-8 py-4">خطا در دریافت داده‌ها: {error}</div>}
                    {loading && <div className="text-center text-sm opacity-70 py-4">در حال بارگذاری…</div>}
                    {!hasMore && !loading && <div className="text-center text-sm opacity-70 py-4">همه موارد بارگذاری شد.</div>}


                    <InfiniteSentinel onHit={() => { if (hasMore && !loading) loadMore(); }} />
                </div>
            </div>
        </div>
    );
}