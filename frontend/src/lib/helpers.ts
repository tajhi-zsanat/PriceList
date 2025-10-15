import { baseURL } from "./api";

// helper for images coming from backend (stored as /uploads/...)
export const imgUrl = (path?: string | null) =>
  path ? `${baseURL}${path}` : undefined;

export const warningSaveProduct = (row: number, title: string | null) => {
  return `در ردیف ${row}، عنوان ${title ?? ""} باید وارد شود تا بتوان محصول را اضافه کرد.`;
};

export const keyFor = (r: number, c: number) => `${r}-${c}`;

export function totalCols(dynamicCount: number) {
  // 2 static on the left + dynamic + 1 static on the right
  const t = 2 + dynamicCount + 1;
  return Math.max(5, Math.min(t, 8));
}

export const resolveImgSrc = (v?: string | null) => {
  if (!v) return "";
  if (v.startsWith("blob:") || v.startsWith("data:") || v.startsWith("http://") || v.startsWith("https://")) {
    return v; // already absolute or object URL
  }
  // ensure exactly one slash between baseURL and path
  try {
    return new URL(v, baseURL).toString();
  } catch {
    return `${baseURL.replace(/\/$/, "")}/${v.replace(/^\//, "")}`;
  }
};