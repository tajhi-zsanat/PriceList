import type { CellKey, GridResponse } from "@/types";

export const keyFor = (r: number, c: number): CellKey => `${r}-${c}` as const;

export function findHeaderIndex(headers: GridResponse["headers"], key: string) {
  return headers.find(h => h.key === key)?.index ?? -1;
}

export function requiredColIndexes(grid: GridResponse) {
  return {
    imageCol: findHeaderIndex(grid.headers, "image"),
    fileCol: findHeaderIndex(grid.headers, "document"),
    descCol: findHeaderIndex(grid.headers, "description"),
    unitCol: findHeaderIndex(grid.headers, "unit"),
    priceCol: findHeaderIndex(grid.headers, "price"),
    moreCol: findHeaderIndex(grid.headers, "more"),
  };
}
