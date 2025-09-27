import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import api from "../../lib/api";
import { imgUrl } from "../../lib/helpers";
import type { CategoryListItemDto } from "../../types";
import Breadcrumbs from "../../components/Breadcrumbs";
import noImage from '../../assets/img/no-image.png';
import loadingImage from '../../assets/img/loading.gif';

export default function Category() {
  const [data, setData] = useState<CategoryListItemDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [err, setErr] = useState<string | null>(null);

  useEffect(() => {
    api.get<CategoryListItemDto[]>("/api/Category")
      .then(r => setData(r.data))
      .catch(e => setErr(e?.response?.data ?? e.message))
      .finally(() => setLoading(false));
  }, []);

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
              هیچ دسته‌ای ثبت نشده است.
            </div>
          ) : (
            <ul className="card-grid">
              {data.map(c => (
                <li
                  key={c.id}
                  className="card-item"
                >
                  <Link
                    to={`/Category/${c.id}/groups`}
                    state={{ categoryName: c.name }}
                    className="card-link"
                    title={`مشاهده گروه‌های ${c.name}`}
                  >
                    <div className="card-media">
                      <img
                        src={c.imagePath ? imgUrl(c.imagePath) : noImage}
                        alt={c.imagePath ? c.name : "no image"}
                        loading="lazy"
                      />
                    </div>
                    <div className="card-title">{c.name}</div>
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