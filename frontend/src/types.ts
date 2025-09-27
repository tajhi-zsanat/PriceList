export type CategoryListItemDto = {
  id: number;
  name: string;
  imagePath: string | null; // present, may be null
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

// Products
export type FeaturesIDs = "__OTHERS__" | string;

export interface ProductHeader {
  id: number;
  value: string | null;
}

/** You didn’t show the shape of productImages items—keep it unknown for now. */
export type ProductImage = unknown;

export interface ProductDto {
  id: number;
  description: string | null;
  documentPath: string | null;
  price: number | null;           // you render null as "--"
  number: number;                 // stock/qty
  productImages: ProductImage[];  // present, possibly empty
  productHeaders: ProductHeader[];// present, possibly empty
}


export interface FeatureBucketDto {
  featuresIDs: FeaturesIDs;       // "__OTHERS__" or "2|3"
  title: string;                  // e.g., "روغنی - خشک"
  products: ProductDto[];
  featureColor: string | null;    // you check for null in UI
}

export interface FeatureBucketsResponse {
  items: FeatureBucketDto[];
  skip: number;
  take: number;
  returnedCount: number;
  totalCount: number | null;      // matches ScrollResult<T>.TotalCount?
  totalProductCount: number;      // you show this in the header
  hasMore: boolean;
}