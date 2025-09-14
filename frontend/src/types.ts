export type CategoryListItemDto = {
  id: number;
  name: string;
  imagePath?: string | null;
};

export type ProductGroupListItemDto = {
  id: number;
  name: string;
  imagePath?: string | null;
};

export type ProductTypeListItemDto = {
  id: number;
  name: string;
  imagePath?: string | null;
};

export type BrandListItemDto = {
  id: number;
  name: string;
  imagePath?: string | null;
};

export interface PaginatedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

export interface ProductCustomPropertyItemDto {
  key: string;
  value: string;
}

export interface ProductListItemDto {
  id: number;
  model?: string | null;
  description?: string | null;
  documentPath?: string | null;
  price?: number | null;
  number?: number | null;
  images: string[];
  customProperties: ProductCustomPropertyItemDto[];
}