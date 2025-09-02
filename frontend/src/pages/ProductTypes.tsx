import { useEffect, useState } from "react";
import { Link, useLocation, useParams } from "react-router-dom";
import api from "../lib/api";
import { imgUrl } from "../lib/helpers";
import type { ProductTypeListItemDto } from "../types";
import Breadcrumbs from "../components/Breadcrumbs";
import noImage from '../assets/img/no-image.png';
import loadingImage from '../assets/img/loading.gif';

export default function ProductTypes() {
  const { categoryId, groupId } = useParams();
  const loc = useLocation() as { state?: { categoryName?: string; groupName?: string } };
  const categoryName = loc.state?.categoryName;
  const groupName = loc.state?.groupName;

  const [data, setData] = useState<ProductTypeListItemDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [err, setErr] = useState<string | null>(null);

  useEffect(() => {
    if (!groupId) return;
    api.get<ProductTypeListItemDto[]>(`/api/ProductType/by-group/${groupId}`)
      .then(r => setData(r.data))
      .catch(e => setErr(e?.response?.data ?? e.message))
      .finally(() => setLoading(false));
  }, [groupId]);

  return (
    <>
      <Breadcrumbs />
      <div className="mt-6 mx-4 sm:mx-16 p-8 rounded-xl bg-white">
        <h1 className="text-xl mb-4">
          نوع‌های {groupName ? `«${groupName}»` : `گروه ${groupId}`}
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
              هیچ نوع کالایی برای این گروه و دسته بندی ثبت نشده است.
            </div>
          ) :
            (
              <ul className="grid sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-6 gap-6">
                {data.map(t => (
                  <li key={t.id} className="bg-white rounded-2xl border border-gray-200 shadow-sm 
                transition-transform duration-200 ease-in-out hover:shadow-md hover:border-gray-400 hover:-translate-y-1"
                  >
                    <Link
                      to={`/categories/${categoryId}/groups/${groupId}/types/${t.id}/brands`}
                      state={{ categoryName, groupName, typeName: t.name }}
                      className="flex flex-col items-center justify-center gap-3 p-4 h-[190px]"
                    >
                      <div className="flex items-center justify-center w-full h-28 overflow-hidden">
                        <img
                          src={t.imagePath ? imgUrl(t.imagePath) : noImage}
                          alt={t.imagePath ? t.name : "no image"}
                          className="max-w-full max-h-full object-contain"
                          loading="lazy"
                        />
                      </div>
                      <div className="text-center text-gray-800">{t.name}</div>
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
