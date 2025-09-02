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
              <ul className="card-grid">
                {data.map(t => (
                  <li key={t.id} className="card-item"
                  >
                    <Link
                      to={`/categories/${categoryId}/groups/${groupId}/types/${t.id}/brands`}
                      state={{ categoryName, groupName, typeName: t.name }}
                      className="card-link"
                    >
                      <div className="card-media">
                        <img
                          src={t.imagePath ? imgUrl(t.imagePath) : noImage}
                          alt={t.imagePath ? t.name : "no image"}
                          loading="lazy"
                        />
                      </div>
                      <div className="card-title">{t.name}</div>
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
