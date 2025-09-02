import { useEffect, useState } from "react";
import { Link, useLocation, useParams } from "react-router-dom";
import api from "../lib/api";
import type { ProductGroupListItemDto } from "../types";
import Breadcrumbs from "../components/Breadcrumbs";
import { imgUrl } from "../lib/helpers";
import noImage from '../assets/img/no-image.png';
import loadingImage from '../assets/img/loading.gif';

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
    <>
      <Breadcrumbs />
      <div className="mt-6 mx-4 sm:mx-16 p-8 rounded-xl bg-white">
        <h1 className="text-xl mb-4">
          گروه‌های {categoryName ? `«${categoryName}»` : `دسته ${categoryId}`}
        </h1>
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
            <ul className="grid sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-6 gap-6">
              {data.map(g => (
                <li
                  key={g.id}
                  className="bg-white rounded-2xl border border-gray-200 shadow-sm 
                  transition-transform duration-200 ease-in-out hover:shadow-md hover:border-gray-400 hover:-translate-y-1"
                >
                  <Link
                    to={`/categories/${categoryId}/groups/${g.id}/types`}
                    state={{ categoryName, groupName: g.name }}
                    className="flex flex-col items-center justify-center gap-3 p-4 h-[190px]"
                  >
                    <div className="flex items-center justify-center w-full h-28 overflow-hidden">
                      <img
                        src={g.imagePath ? imgUrl(g.imagePath) : noImage}
                        alt={g.imagePath ? g.name : "no image"}
                        className="max-w-full max-h-full object-contain"
                        loading="lazy"
                      />
                    </div>
                    <div className="text-center text-gray-800">{g.name}</div>
                  </Link>
                </li>
              ))}
            </ul>
          )
        )}
      </div>
    </>
  );
}
