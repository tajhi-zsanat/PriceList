// components/admin/CreateFormModal.tsx
import { useState, useCallback } from "react";
import {
  Dialog, DialogContent, DialogHeader, DialogTitle, DialogDescription,
  DialogFooter, DialogTrigger, DialogClose
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";
import { Input } from "@/components/ui/input";
import type { CreateFormModalProps, Item, LoadItemsFn } from "@/types";
import api from "@/lib/api";
import EntityPickerDialog from "@/components/admin/EntityPickerDialog";

export default function CreateFormModal({ open, onOpenChange, trigger, onSubmit }: CreateFormModalProps) {
  const [title, setTitle] = useState("");
  const [columns, setColumns] = useState<number>(6);
  const [rows, setRows] = useState<number>(0);

  const [openCategory, setOpenCategory] = useState(false);
  const [openGroup, setOpenGroup] = useState(false);
  const [openType, setOpenType] = useState(false);

  const [selectedCategory, setSelectedCategory] = useState<Item | null>(null);
  const [selectedGroup, setSelectedGroup] = useState<Item | null>(null);
  const [selectedType, setSelectedType] = useState<Item | null>(null);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();

    console.log(selectedType);

    onSubmit?.({
      title: title.trim(),
      columns,
      // include these IDs in your payload as needed:
      // categoryId: selectedCategory?.id,
      // groupId: selectedGroup?.id,
      // typeId: selectedType?.id,
      // rows,
    });
  };

  // --- Loaders (adapt endpoints to your API) ---
  const loadCategories: LoadItemsFn<Item> = useCallback(async ({ search, signal }) => {
    const r = await api.get<Item[]>("/api/Category", {
      params: { q: search || undefined },
      signal,
    });
    return r.data;
  }, []);

  const loadGroups: LoadItemsFn<Item> = useCallback(async ({ search, signal }) => {
    if (!selectedCategory) return [];
    const r = await api.get<Item[]>("/api/ProductGroup/by-category", {
      params: { categoryId: selectedCategory.id, q: search || undefined },
      signal,
    });
    return r.data;
  }, [selectedCategory]);

  const loadTypes: LoadItemsFn<Item> = useCallback(async ({ search, signal }) => {
    if (!selectedGroup) return [];
    const r = await api.get<Item[]>("/api/ProductType/by-group", {
      params: { groupId: selectedGroup.id, q: search || undefined },
      signal,
    });
    return r.data;
  }, [selectedGroup]);

  return (
    <>
      <Dialog open={open} onOpenChange={onOpenChange}>
        <DialogTrigger asChild>{trigger}</DialogTrigger>

        <DialogContent dir="rtl" className="sm:max-w-[520px]">
          <DialogHeader className="text-start gap-4">
            <DialogTitle className="border-b border-b-[#CFD8DC] pb-4">افزودن فرم جدید</DialogTitle>
            <DialogDescription className="leading-6">
              ابتدا دسته‌بندی، سپس گروه و نوع کالا را انتخاب کنید. بعد، سطر/ستون جدول را مشخص کنید.
            </DialogDescription>
          </DialogHeader>

          <form onSubmit={handleSubmit} className="space-y-4">
            {/* Chain opener */}
            <div
              className="flex justify-between bg-[#F5F5F5] p-2 rounded-[8px] text-[#636363] cursor-pointer"
              onClick={() => setOpenCategory(true)}
              title="انتخاب دسته‌بندی / گروه / نوع"
            >
              <span className="truncate">
                {selectedCategory?.name ?? "انتخاب دسته‌بندی"}
                {selectedGroup ? ` / ${selectedGroup.name}` : ""}
                {selectedType ? ` / ${selectedType.name}` : ""}
              </span>
              <span>انتخاب</span>
            </div>

            <div className="grid gap-2">
              <Label htmlFor="formTitle">عنوان فرم</Label>
              <Input
                id="formTitle"
                placeholder="مثلاً: لیست قیمت گیج فشار"
                value={title}
                onChange={(e) => setTitle(e.target.value)}
                required
              />
            </div>

            <div className="flex flex-col md:flex-row justify-between items-center gap-2">
              <div className="flex-1 grid gap-2">
                <Label htmlFor="columns">تعداد ستون‌ (۶ تا ۹)</Label>
                <Input
                  id="columns"
                  type="number"
                  min={6}
                  max={9}
                  value={columns}
                  onChange={(e) => {
                    const v = Number(e.target.value);
                    isNaN(v) ? setColumns(6) : setColumns(Math.min(9, Math.max(6, v)));
                  }}
                  required
                />
              </div>

              <div className="flex-1 grid gap-2">
                <Label htmlFor="rows">تعداد ردیف</Label>
                <Input
                  id="rows"
                  type="number"
                  min={0}
                  value={rows}
                  onChange={(e) => {
                    const v = Number(e.target.value);
                    isNaN(v) ? setRows(0) : setRows(Math.max(0, v));
                  }}
                  required
                />
              </div>
            </div>

            <DialogFooter className="gap-3">
              <DialogClose className="flex-1 border-[#1F78AE] text-[#1F78AE]" asChild>
                <Button type="button" variant="outline">انصراف</Button>
              </DialogClose>

              <Button className="bg-[#1F78AE]" type="submit">ایجاد فرم</Button>
            </DialogFooter>
          </form>
        </DialogContent>
      </Dialog>

      {/* Category Picker */}
      <EntityPickerDialog<Item>
        open={openCategory}
        onOpenChange={(v) => {
          setOpenCategory(v);
          // if user closed manually, don't auto-open next
        }}
        title="انتخاب دسته‌بندی"
        loadItems={loadCategories}
        onSelect={(cat) => {
          setSelectedCategory(cat);
          setSelectedGroup(null);
          setSelectedType(null);
          // open next step
          setOpenGroup(true);
        }}
      />

      {/* Group Picker (depends on selected category) */}
      <EntityPickerDialog<Item>
        open={openGroup}
        onOpenChange={setOpenGroup}
        title="انتخاب گروه"
        loadItems={loadGroups}
        onSelect={(grp) => {
          setSelectedGroup(grp);
          setSelectedType(null);
          // open next step
          setOpenType(true);
        }}
      />

      {/* Type Picker (depends on selected group) */}
      <EntityPickerDialog<Item>
        open={openType}
        onOpenChange={setOpenType}
        title="انتخاب نوع"
        loadItems={loadTypes}
        onSelect={(typ) => {
          setSelectedType(typ);
          // finish chain
        }}
      />
    </>
  );
}
