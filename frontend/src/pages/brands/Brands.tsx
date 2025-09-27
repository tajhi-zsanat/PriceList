import { useEffect, useState } from "react";
import { Link, useLocation, useParams } from "react-router-dom";
import api from "../../lib/api";
import { imgUrl } from "../../lib/helpers";
import type { BrandListItemDto } from "../../types";
import noImage from '../../assets/img/no-image.png';
import loadingImage from '../../assets/img/loading.gif';
import Breadcrumbs from "../../components/Breadcrumbs";

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
              هیچ برندی برای این گروه ،  نوع و دسته بندی ثبت نشده است.
            </div>
          ) :
            (
              <ul className="card-grid">
                {data.map(b => (
                  <li key={b.id} className="card-item"
                  >
                    <Link
                      to={`/Category/${categoryId}/groups/${groupId}/types/${typeId}/brands/${b.id}/products`}
                      state={{ categoryName, groupName, typeName, brandName: b.name }}
                      className="card-link"
                    >
                      <div className="flex items-center justify-center w-full h-28 overflow-hidden">
                        <img
                          src={b.imagePath ? imgUrl(b.imagePath) : noImage}
                          alt={b.imagePath ? b.name : "no image"}
                          loading="lazy"
                        />
                      </div>
                      <div className="card-title">{b.name}</div>
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
