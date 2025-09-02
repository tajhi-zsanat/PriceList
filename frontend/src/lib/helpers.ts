import { baseURL } from "./api";

// helper for images coming from backend (stored as /uploads/...)
export const imgUrl = (path?: string | null) =>
  path ? `${baseURL}${path}` : undefined;
