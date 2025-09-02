import { useEffect, useState } from "react";
import { Link, useLocation, useParams } from "react-router-dom";
import api from "../lib/api";
import { imgUrl } from "../lib/helpers";
import type { BrandListItemDto } from "../types";
import Breadcrumbs from "../components/Breadcrumbs";
import noImage from '../assets/img/no-image.png';
import loadingImage from '../assets/img/loading.gif';

export default function Brands() {
  const { categoryId, groupId, typeId } = useParams();
  const loc = useLocation() as {
    state?: { categoryName?: string; groupName?: string; typeName?: string };
  };
  const { categoryName, groupName, typeName } = loc.state || {};

  const [data, setData] = useState<BrandListItemDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [err, setErr] = useState<string | null>(null);

  useEffect(() => {
    if (!categoryId || !groupId || !typeId) return;
    api.get<BrandListItemDto[]>("/api/Brand/by-categories", {
      params: { categoryId, groupId, typeId },
    })
      .then(r => setData(r.data))
      .catch(e => setErr(e?.response?.data ?? e.message))
      .finally(() => setLoading(false));
  }, [categoryId, groupId, typeId]);

  return (
    <>
      <Breadcrumbs />
      <div className="mt-6 mx-4 sm:mx-16 p-8 rounded-xl bg-white">
        <h1 className="text-xl mb-4">
          برندها برای {typeName ? `«${typeName}»` : `نوع ${typeId}`}
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
              هیچ برندی برای این گروه ،  نوع و دسته بندی ثبت نشده است.
            </div>
          ) :
            (
              <ul className="grid sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-6 gap-6">
                {data.map(b => (
                  <li key={b.id} className="bg-white rounded-2xl border border-gray-200 shadow-sm 
                transition-transform duration-200 ease-in-out hover:shadow-md hover:border-gray-400 hover:-translate-y-1"
                  >
                    <Link
                      to={`/categories/${categoryId}/groups/${groupId}/types/${typeId}/brands/${b.id}/products`}
                      state={{ categoryName, groupName, typeName, brandName: b.name }}
                      className="flex flex-col items-center justify-center gap-3 p-4 h-[190px]"
                    >
                      <div className="flex items-center justify-center w-full h-28 overflow-hidden">
                        <img
                          src={b.imagePath ? imgUrl(b.imagePath) : noImage}
                          alt={b.imagePath ? b.name : "no image"}
                          className="max-w-full max-h-full object-contain"
                          loading="lazy"
                        />
                      </div>
                      <div className="text-center text-gray-800">{b.name}</div>
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
