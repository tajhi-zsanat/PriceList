// components/admin/CreateFormModal.tsx
import { useState, useCallback, useEffect } from "react";
import {
  Dialog, DialogContent, DialogHeader, DialogTitle, DialogDescription,
  DialogFooter, DialogTrigger, DialogClose
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Label } from "@/components/ui/label";
import { Input } from "@/components/ui/input";
import type { CreateFormModalProps, FormCreateDto, Item, LoadItemsFn } from "@/types";
import api from "@/lib/api";
import EntityPickerDialog from "@/components/admin/EntityPickerDialog";
import { toast } from "sonner";

export default function CreateFormModal({ open, onOpenChange, trigger, onCreated = () => { } }: CreateFormModalProps) {
  const [title, setTitle] = useState("");
  const [columns, setColumns] = useState<number>(5);
  const [rows, setRows] = useState<number>(1);
  const [displayOrder, setDisplayOrder] = useState<number>(0);

  const [openCategory, setOpenCategory] = useState(false);
  // const [openType, setOpenType] = useState(false);
  const [openBrand, setOpenBrand] = useState(false);

  const [selectedCategory, setSelectedCategory] = useState<Item | null>(null);
  // const [selectedType, setSelectedType] = useState<Item | null>(null);
  const [selectedBrand, setSelectedBrand] = useState<Item | null>(null);

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (open) {
      setTitle("");
      setColumns(5);
      setRows(1);
      setDisplayOrder(0);

      setSelectedCategory(null);
      // setSelectedType(null);
      setSelectedBrand(null);

      setError(null);
      setLoading(false);

      setOpenCategory(false);
      // setOpenType(false);
      setOpenBrand(false);
    }
  }, [open]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    if (!selectedCategory) {
      setError("دسته‌بندی را انتخاب کنید.");
      return;
    }

    if (!selectedBrand) {
      setError("برند را انتخاب کنید.");
      return;
    }

    if (displayOrder === 0) {
      setError("ترتیب نمایش را انتخاب کنید.");
      return;
    }

    const allCols: number = columns + 3;

    const payload: FormCreateDto = {
      formTitle: title.trim(),
      columns: allCols,
      categoryId: selectedCategory?.id ?? null,
      brandId: selectedBrand?.id,
      rows,
      displayOrder,
    };

    try {
      setLoading(true);
      const res = await api.post<FormCreateDto>("/api/Form", payload);

      if (res.status === 201) {
        toast.success("فرم با موفقیت ایجاد شد ✅");
        onCreated();
        onOpenChange?.(false);
      }

    } catch (err: any) {
      console.error("Create form failed:", err);
      // extract message if available
      toast.error(err.response.data);
    } finally {
      setLoading(false);
    }
  };

  // --- Loaders (adapt endpoints to your API) ---
  const loadCategories: LoadItemsFn<Item> = useCallback(async ({ search, signal }) => {
    const r = await api.get<Item[]>("/api/Category", {
      params: { q: search || undefined },
      signal,
    });
    return r.data;
  }, []);

  // const loadTypes: LoadItemsFn<Item> = useCallback(async ({ search, signal }) => {
  //   if (!selectedGroup) return [];
  //   const r = await api.get<Item[]>("/api/ProductType/by-group", {
  //     params: { groupId: selectedGroup.id, q: search || undefined },
  //     signal,
  //   });
  //   return r.data;
  // }, [selectedGroup]);

  const loadBrands: LoadItemsFn<Item> = useCallback(async ({ search, signal }) => {
    const r = await api.get<Item[]>("/api/Brand", {
      params: { q: search || undefined },
      signal,
    });
    return r.data;
  }, [selectedBrand]);

  return (
    <>
      <Dialog open={open} onOpenChange={onOpenChange}>
        <DialogTrigger asChild>{trigger}</DialogTrigger>

        <DialogContent
          onOpenAutoFocus={(e) => e.preventDefault()}
          dir="rtl"
          className="">
          <DialogHeader className="text-start gap-4">
            <DialogTitle className="border-b border-b-[#CFD8DC] pb-4">افزودن فرم جدید</DialogTitle>
            <DialogDescription className="leading-6">
              ابتدا دسته‌بندی، سپس گروه و نوع کالا را انتخاب کنید. بعد، سطر/ستون جدول را مشخص کنید.
            </DialogDescription>
          </DialogHeader>

          <form onSubmit={handleSubmit} className="space-y-4">
            {error && <div className="text-red-600 text-sm">{error}</div>}
            {/* Chain opener */}
            <div
              className="flex justify-between bg-[#F5F5F5] p-2 rounded-[8px] text-[#636363] cursor-pointer"
              onClick={() => setOpenCategory(true)}
              title="انتخاب دسته‌بندی / گروه / نوع"
            >
              <span className="truncate">
                {selectedCategory?.name ?? "انتخاب دسته‌بندی"}
                {/* {selectedType ? ` / ${selectedType.name}` : ""} */}
              </span>
              <span>انتخاب</span>
            </div>

            <div
              className="flex justify-between bg-[#F5F5F5] p-2 rounded-[8px] text-[#636363] cursor-pointer"
              onClick={() => setOpenBrand(true)}
              title="انتخاب برند"
            >
              <span className="truncate">
                {selectedBrand ? ` ${selectedBrand.name}` : "انتخاب برند"}
              </span>
              <span>انتخاب</span>
            </div>

            <div className="flex flex-col md:flex-row justify-between items-center gap-2">
              <div className="flex-1 grid gap-2">
                <Label htmlFor="formTitle">عنوان فرم</Label>
                <Input
                  className="bg-[#f5f5f5]"
                  id="formTitle"
                  placeholder="مثلاً: لیست قیمت گیج فشار"
                  value={title}
                  onChange={(e) => setTitle(e.target.value)}
                />
              </div>

              <div className="flex-1 grid gap-2">
                <Label htmlFor="rows">ترتیب نمایش</Label>
                <Input
                  className="bg-[#f5f5f5]"
                  id="displayOrder"
                  type="number"
                  min={0}
                  value={displayOrder}
                  onChange={(e) => {
                    const v = Number(e.target.value);
                    isNaN(v) ? setDisplayOrder(0) : setDisplayOrder(Math.max(0, v));
                  }}
                />
              </div>
            </div>

            <div className="flex flex-col md:flex-row justify-between items-center gap-2">
              <div className="flex-1 grid gap-2">
                <Label htmlFor="columns">تعداد ستون‌ (5 تا 8)</Label>
                <Input
                  className="bg-[#f5f5f5]"
                  id="columns"
                  type="number"
                  min={5}
                  max={8}
                  value={columns}
                  onChange={(e) => {
                    const v = Number(e.target.value);
                    isNaN(v) ? setColumns(5) : setColumns(Math.min(8, Math.max(5, v)));
                  }}
                  required
                />
              </div>

              <div className="flex-1 grid gap-2">
                <Label htmlFor="rows">تعداد ردیف</Label>
                <Input
                  className="bg-[#f5f5f5]"
                  id="rows"
                  type="number"
                  min={1}
                  max={100}
                  value={rows}
                  onChange={(e) => {
                    const v = Number(e.target.value);
                    isNaN(v) ? setRows(1) : setRows(Math.min(100, Math.max(1, v)));
                  }}
                  required
                />
              </div>
            </div>

            <DialogFooter className="gap-3">
              <Button
                className="flex-1 cursor-pointer bg-[#1F78AE] hover:bg-[#0a4c75]"
                type="submit" disabled={loading}>
                {loading ? "در حال انجام..." : "ایجاد فرم"}
              </Button>

              <DialogClose
                className="flex-1 cursor-pointer border-[#1F78AE] text-[#1F78AE] hover:bg-[#1F78AE] hover:text-white" asChild>
                <Button type="button" variant="outline">انصراف</Button>
              </DialogClose>
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
          // setSelectedType(null);
          // open next step
        }}
      />


      {/* Type Picker (depends on selected group) */}
      {/* <EntityPickerDialog<Item>
        open={openType}
        onOpenChange={setOpenType}
        title="انتخاب نوع"
        loadItems={loadTypes}
        onSelect={(typ) => {
          setSelectedType(typ);
          // finish chain
        }}
      /> */}

      {/* Brand Picker (Load All Brands) */}
      <EntityPickerDialog<Item>
        open={openBrand}
        onOpenChange={setOpenBrand}
        title="انتخاب برند"
        loadItems={loadBrands}
        onSelect={(b) => {
          setSelectedBrand(b);
          // finish chain
        }}
      />
    </>
  );
}
