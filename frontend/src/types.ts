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
export type ProductImage = string;

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

export type UseInfiniteProductsArgs = {
  params: { categoryId?: string; groupId?: string; typeId?: string; brandId?: string };
  take?: number;
};

export type PriceToolbarProps = {
  onPdf?: () => void;
  onPrint?: () => void;
  onNotify?: () => void;
  vatChecked?: boolean;
  onVatChange?: (v: boolean) => void;
};

export type FeatureChipsProps = {
  data: FeatureBucketsResponse | null
};

export type SearchInputProps = {
  placeholder?: string;
  value: string;
  onChange: (v: string) => void;
  className?: string;
};

export interface Bucket {
  featuresIDs: string;
  title: string;
  featureColor?: string | null;
  products: any[];
}

export interface ProductTableProps {
  buckets: Bucket[];
  headers: ProductHeader[];
}

export interface SectionProps {
  title: string;
  color?: string | null;
  colSpan: number;
}

export type InfiniteSentinelProps = {
  onHit: () => void;
  rootMargin?: string;
  threshold?: number
};

export type AdminHeaderProps = {
  onFormCreated?: () => void;
};

export type FormsContextType = {
  data: FormListItemDto[];
  loading: boolean;
  error: string | null;
  reload: () => Promise<void> | void;
};

export type FormListItemDto = {
  id: number;
  formTitle: string;
  productCount: number;
  categoryName: string;
  groupdName: string;
  typeName: string;
  brandName: string;
  updatedDate: string;
};

export type FormCreateDto = {
  formTitle: string;
  columns: number;
  categoryId?: number;
  groupId?: number;
  typeId?: number;
  brandId?: number;
  rows?: number;
  displayOrder?: number;
};

export type CreateFormModalProps = {
  open?: boolean;
  onOpenChange?: (v: boolean) => void;
  trigger: React.ReactNode;
  onCreated?: () => void; // parent decides what happens after creation
};

export type Item = { id: number; name: string };

export type LoadItemsFn<T> = (args: { search: string; signal: AbortSignal }) => Promise<T[]>;

export type BaseItem = { id: number | string; name: string };

export type EntityPickerDialogProps<TItem extends BaseItem> = {
  open: boolean;
  onOpenChange: (v: boolean) => void;
  title: string;
  // ⬇️ change to the new signature
  loadItems: LoadItemsFn<TItem>;
  onSelect: (item: TItem) => void;
  placeholder?: string;
  renderRow?: (item: TItem) => React.ReactNode;
};

export interface FormForProductsDto {
  id: number;
  brandId: number;
  categoryId: number;
  productGroupId: number;
  productTypeId: number;
  supplierId: number;
  rows: number;
  column: number;
}

export interface ProductCreateForm {
  description: string;

  documentPath?: string | null;
  price: number;
  categoryId: number;
  productGroupId: number;
  productTypeId: number;
  brandId: number;
  supplierId: number;
  unitId: number;
  image?: File[] | null;
}


// FormDate

// enums help you stay consistent with server
export const ColumnKinds = ["Static", "Fixed", "Dynamic"] as const;
export type ColumnKind = typeof ColumnKinds[number];

export const ColumnTypes = [
  "Checkbox",
  "Rowno",
  "Image",
  "File",
  "MultilineText",
  "Select",
  "Custom1",
  "Custom2",
  "Custom3",
  "Price",
  "More",
] as const;
export type ColumnType = typeof ColumnTypes[number];

// header for each column
export interface FormHeader {
  index: number;
  kind: ColumnKind;              // "Static" | "Fixed" | "Dynamic"
  type: ColumnType | string;     // allow future/unknown types from backend
  key: string;                   // e.g. "image" | "unit" | ...
  title: string;                 // localized title
}

// a single cell in a row
export interface GridCell {
  id: number;
  colIndex: number;
  value: string | number | boolean | null;
}

// a row
export interface GridRow {
  rowId: number;
  cells: GridCell[];
}

// server groups rows by featureNames (can be empty [])
export interface GridGroup {
  featureNames: string[];
  rows: GridRow[];
  count: number;
}

// full payload
export interface GridResponse {
  headers: FormHeader[];
  cells: GridGroup[];
}

// convenience
// Nice typed key like "12-5"
export type CellKey = `${number}-${number}`;
export const keyFor = (r: number, c: number): CellKey => `${r}-${c}`;

