import api from "@/lib/api";
import type { GridResponse } from "@/types";

export async function getGrid(formId: number | string, signal?: AbortSignal) {
  const { data } = await api.get<GridResponse>(`/api/Form/${formId}/allCells`, { signal });
  return data;
}

export interface UpsertCellRequest {
  id: number | undefined;
  value: string | number | boolean | null;
}

export async function upsertCell(payload: UpsertCellRequest) {
  // match your new backend contract if it differs
  await api.put(`/api/Form/Cell`, payload);
}

export async function uploadImage(id: number, file: File) {
  const fd = new FormData();
  fd.append("file", file);
  fd.append("id", id.toString()); // Convert to string for FormData

  const { data } = await api.put(`/api/Form/UploadImage`, fd, {
    headers: { "Content-Type": "multipart/form-data" },
  });

  // expect { imageUrl: string }
  return data?.imageUrl as string | undefined;
}
