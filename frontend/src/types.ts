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

export interface ProductListItemDto {
  id: number;
  model?: string | null;
  description?: string | null;
  imagePath?: string | null;
  price?: number | null;
}

export interface SupplierSummaryDto {
  supplierId: number;
  supplierName: string;
  productCount: number;
}

export interface SupplierProductsPageDto {
  supplierId: number;
  supplierName: string;
  items: ProductListItemDto[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

// local React state helpers
export type LocState = {
  categoryName?: string;
  groupName?: string;
  typeName?: string;
  brandName?: string;
};

export type SupplierSection = {
  supplierId: number;
  supplierName: string;
  items: ProductListItemDto[];
  page: number;        // current loaded page
  hasNext: boolean;    // more pages for this supplier?
  totalCount: number;  // optional, for UI
};