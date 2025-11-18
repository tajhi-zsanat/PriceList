import api from "@/lib/api";
import type { GridResponse } from "@/types";

// ---------- Types for responses ----------
interface UploadImageResponse { imageUrl: string }   // <-- match backend casing
interface UploadPdfResponse { pdfUrl: string }     // <-- pick one casing and keep it!

// ---------- Queries / commands ----------
export async function getGrid(
  formId: number | string,
  page: number,
  pageSize: number,
  signal?: AbortSignal
): Promise<GridResponse> {
  const { data } = await api.get<GridResponse>(
    `/api/Form/${formId}/cells`,
    { params: { page, pageSize }, signal }
  );
  return data;
}

export interface UpsertCellRequest {
  formId: string | null;
  id: number;
  value: string | number | boolean | null;
}

export interface UpsertHeaderCellRequest {
  formId: number;
  Index: number;
  value: string | number | boolean | null;
}

export interface AddColDefPayload {
  formId: number;
  customColDef: string[];
}

export interface AddRowPayload {
  featureId: number;
  rowIndex: number;
  formId: string | null;
}

export interface RemoveRowPayload {
  formId: string | null;
  rowIndex: number;
}

export interface removeHeaderDefRequest {
  formId: number;
  index: number;
}

export interface AddFeatureToRowsRequest {
  formId: string | number;
  feature: string | undefined;
  rowIds: number[];
  displayOrder: string;
  color?: string;
}

export async function upsertCell(payload: UpsertCellRequest): Promise<void> {
  await api.put(`/api/Form/Cell`, payload);
}

export async function upsertHeaderCell(payload: UpsertHeaderCellRequest): Promise<void> {
  await api.put(`/api/Form/headerCell`, payload);
}

export async function RemoveCellMedia(payload: UpsertCellRequest): Promise<void> {
  await api.put(`/api/Form/RemoveMediaCell`, payload);
}

export async function removeDef(payload: removeHeaderDefRequest): Promise<void> {
  try {
    await api.delete(`/api/Form/DeleteColDef`, { data: payload });
  } catch (err: any) {
    console.error("Delete failed", err);
    throw err; // or handle gracefully
  }
}

// ---------- Uploads (always return string) ----------
export async function uploadImage(formId: string | null, id: number, file: File): Promise<string> {
  const fd = new FormData();
  fd.append("file", file);
  fd.append("id", String(id));
  fd.append("formId", String(formId));

  const { data } = await api.put<UploadImageResponse>(`/api/Form/UploadImage`, fd, {
    headers: { "Content-Type": "multipart/form-data" },
  });

  if (!data?.imageUrl) throw new Error("Invalid uploadImage response");
  return data.imageUrl;
}

export async function uploadPDF(formId: string | null, id: number, file: File): Promise<string> {
  const fd = new FormData();
  fd.append("file", file);
  fd.append("id", String(id));
  fd.append("formId", String(formId));

  const { data } = await api.put<UploadPdfResponse>(`/api/Form/UploadFile`, fd, {
    headers: { "Content-Type": "multipart/form-data" },
  });

  if (!data?.pdfUrl) throw new Error("Invalid uploadPDF response");
  return data.pdfUrl;
}

export async function AddColDef(payload: AddColDefPayload) {
  // Axios throws on non-2xx automatically
  return api.post(`/api/Form/CreateColDef`, payload);
}

export async function addFeatureToRows(
  payload: AddFeatureToRowsRequest,
  signal?: AbortSignal) {
  return api.post(`/api/Form/assignments`
    , payload
    , { signal });
}

export async function AddRow(payload: AddRowPayload) {
  // Axios throws on non-2xx automatically
  return api.post(`/api/Form/CreateRow`, payload);
}

export async function RemoveRow(payload: RemoveRowPayload) {
  return api.delete(`/api/Form/DeleteRow`, { data: payload });
}