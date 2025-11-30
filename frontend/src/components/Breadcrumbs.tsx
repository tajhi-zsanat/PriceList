import { Link, useParams, useLocation } from "react-router-dom";

export default function Breadcrumbs() {
  const { categoryId, groupId, typeId, brandId } = useParams();
  // Each page passes names via state
  const loc = useLocation() as {
    state?: {
      categoryName?: string;
      groupName?: string;
      typeName?: string;
      brandName?: string;
    };
  };

  const s = loc.state || {};

  return (
    <nav className="text-sm text-gray-600 flex flex-wrap gap-x-2 gap-y-1 sm:mx-16 pt-4">
      <Link to="/category" state={{ categoryName: s.categoryName }}>
        دسته‌بندی‌ها
      </Link>
      {categoryId && (
        <>
          <span>/</span>
          <Link to={`/Category/${categoryId}/groups`} state={{ categoryName: s.categoryName }}>
            {s.categoryName ?? `دسته ${categoryId}`}
          </Link>
        </>
      )}
      {groupId && (
        <>
          <span>/</span>
          <Link
            to={`/Category/${categoryId}/groups/${groupId}/brands`}
            state={{ categoryName: s.categoryName, groupName: s.groupName }}
          >
            {s.groupName ?? `گروه ${groupId}`}
          </Link>
        </>
      )}
      {typeId && (
        <>
          <span>/</span>
          <Link
            to={`/Category/${categoryId}/groups/${groupId}/brands`}
            state={{ categoryName: s.categoryName, groupName: s.groupName, typeName: s.typeName }}
          >
            {s.typeName ?? `نوع ${typeId}`}
          </Link>
        </>
      )}
      {brandId && (
        <>
          <span>/</span>
          <span>{s.brandName ?? `برند ${brandId}`}</span>
        </>
      )}
    </nav>
  );
}
