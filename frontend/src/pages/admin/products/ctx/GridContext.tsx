// ctx/GridContext.tsx
import type { Ctx, FilesMap, ValuesMap, ValuesMapHeader } from "@/types";
import { createContext, useContext, useMemo, useState } from "react";


const GridContext = createContext<Ctx | null>(null);
export const useGridCtx = () => {
  const ctx = useContext(GridContext);
  if (!ctx) throw new Error("GridContext missing");
  return ctx;
};

export function GridProvider({ children }: { children: React.ReactNode }) {
  const [editing, setEditing] = useState<number | null>(null);
  const [editingHeader, setEditingHeader] = useState<string | null>(null);
  const [editValue, setEditValue] = useState("");
  const [cellValues, setCellValues] = useState<ValuesMap>({});
  const [cellValuesHeader, setcellValuesHeader] = useState<ValuesMapHeader>({});
  const [files, setFiles] = useState<FilesMap>({});
  const [rowIds, setRowIds] = useState<Record<number, number>>({});

  const value = useMemo(() => ({
    editing, setEditing, editingHeader, setEditingHeader
    , editValue, setEditValue, files, setFiles
    , cellValues, setCellValues, cellValuesHeader, setcellValuesHeader, rowIds, setRowIds
  }), [editing, editingHeader, editValue, files, cellValues, cellValuesHeader, setcellValuesHeader, rowIds, setRowIds]);

  return <GridContext.Provider value={value}>{children}</GridContext.Provider>;
}
