import { useEffect, useState } from "react";
import { Link, useLocation, useParams } from "react-router-dom";
import api from "@/lib/api";
import type { ProductGroupListItemDto } from "@/types";
import Breadcrumbs from "@/components/Breadcrumbs";
import { imgUrl } from "@/lib/helpers";
import noImage from '@/assets/img/no-image.png';
import loadingImage from '@/assets/img/loading.gif';

export default function ProductGroups() {
  const { categoryId } = useParams();
  const loc = useLocation() as { state?: { categoryName?: string } };
  const categoryName = loc.state?.categoryName;
  
  const [data, setData] = useState<ProductGroupListItemDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [err, setErr] = useState<string | null>(null);

  useEffect(() => {
    if (!categoryId) return;
    api.get<ProductGroupListItemDto[]>(`/api/ProductGroup/by-category/${categoryId}`)
      .then(r => setData(r.data))
      .catch(e => setErr(e?.response?.data ?? e.message))
      .finally(() => setLoading(false));
  }, [categoryId]);

  return (
    <div className="flex-1 bg-[#F5F5F5]">
      <Breadcrumbs />
      <div className="mt-6 mx-4 sm:mx-16 p-8 rounded-xl bg-white">
        {loading && <div>
          <img
            className=""
            src={loadingImage}
            alt='Loading'
            loading="lazy"
          />
        </div>}
        {err && <div className="text-red-600">خطا: {err}</div>}
        {!loading && !err && (
          data.length === 0 ? (
            <div className="text-gray-500 text-center py-8">
              هیچ گروهی برای این دسته‌بندی پیدا نشد.
            </div>
          ) : (
            <ul className="card-grid">
              {data.map(g => (
                <li
                  key={g.id}
                  className="card-item"
                >
                  <Link
                    to={`/category/${categoryId}/groups/${g.id}/types`}
                    state={{ categoryName, groupName: g.name }}
                    className="card-link"
                  >
                    <div className="card-media">
                      <img
                        src={g.imagePath ? imgUrl(g.imagePath) : noImage}
                        alt={g.imagePath ? g.name : "no image"}
                        loading="lazy"
                      />
                    </div>
                    <div className="card-title">{g.name}</div>
                  </Link>
                </li>
              ))}
            </ul>
          )
        )}
      </div>
    </div>
  );
}
