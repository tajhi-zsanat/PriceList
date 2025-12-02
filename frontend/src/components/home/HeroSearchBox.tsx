import { useEffect, useState, type JSX } from "react";
import { Link } from "react-router-dom";

import FilterButton from "./FilterButton";
import api from "@/lib/api";
import type {
  BrandListItemDto,
  CategoryListItemDto,
  ProductGroupListItemDto,
  SelectedValues,
} from "@/types";

import categoryIcon from "@/assets/img/home/category.png";
import groupIcon from "@/assets/img/home/groupIcon.png";
import brandIcon from "@/assets/img/home/brnadIcon.png";

interface HeroSearchBoxProps {
  arrowDownIcon: string;
}

export default function HeroSearchBox({
  arrowDownIcon,
}: HeroSearchBoxProps): JSX.Element {
  const [categoryData, setCategoryData] = useState<CategoryListItemDto[]>([]);
  const [groupData, setGroupData] = useState<ProductGroupListItemDto[]>([]);
  const [brandData, setBrandData] = useState<BrandListItemDto[]>([]);

  const [selected, setSelected] = useState<SelectedValues>({
    categoryId: null,
    groupId: null,
    brandId: null,
  });

  const [loadingCategory, setLoadingCategory] = useState<boolean>(false);
  const [loadingGroup, setLoadingGroup] = useState<boolean>(false);
  const [loadingBrand, setLoadingBrand] = useState<boolean>(false);

  const [isOpenCategory, setIsOpenCategory] = useState(false);
  const [isOpenGroup, setIsOpenGroup] = useState(false);
  const [isOpenBrand, setIsOpenBrand] = useState(false);

  useEffect(() => {
    setLoadingCategory(true);

    api
      .get<CategoryListItemDto[]>("/api/Category")
      .then((r) => setCategoryData(r.data))
      .catch((e) => console.log(e?.response?.data ?? e.message))
      .finally(() => setLoadingCategory(false));
  }, []);

  useEffect(() => {
    const categoryId = selected.categoryId;

    setGroupData([]);
    setBrandData([]);
    setSelected((prev) => ({
      ...prev,
      groupId: null,
      brandId: null,
    }));

    if (!categoryId) return;

    setLoadingGroup(true);

    api
      .get<ProductGroupListItemDto[]>("/api/ProductGroup/by-category", {
        params: { categoryId },
      })
      .then((r) => setGroupData(r.data))
      .catch((e) => console.log(e?.response?.data ?? e.message))
      .finally(() => setLoadingGroup(false));
  }, [selected.categoryId]);

  useEffect(() => {
    const categoryId = selected.categoryId;
    const groupId = selected.groupId;

    setBrandData([]);
    setSelected((prev) => ({
      ...prev,
      brandId: null,
    }));

    if (!categoryId || !groupId) return;

    setLoadingBrand(true);

    api
      .get<BrandListItemDto[]>("/api/Brand/by-categories", {
        params: { categoryId, groupId },
      })
      .then((r) => setBrandData(r.data))
      .catch((e) => console.log(e?.response?.data ?? e.message))
      .finally(() => setLoadingBrand(false));
  }, [selected.groupId, selected.categoryId]);

  const selectedCategory = categoryData.find(
    (c) => c.id === selected.categoryId
  );
  const selectedGroup = groupData.find((g) => g.id === selected.groupId);
  const selectedBrand = brandData.find((b) => b.id === selected.brandId);

  const canSearch =
    !!selected.categoryId && !!selected.groupId && !!selected.brandId;

  const searchUrl = canSearch
    ? `/Category/${selected.categoryId}/groups/${selected.groupId}/brands/${selected.brandId}/products`
    : "#";

  return (
    <div
      className="
        absolute right-1/2 translate-x-1/2
        max-w-6xl w-full -bottom-16 bg-white rounded-xl p-4
        border border-[#CFD8DC]
        shadow-[0px_0px_16px_0px_#D9D9D9]
      "
      dir="rtl"
    >
      <p className="font-medium">محصول مورد نظر خود را انتخاب کنید</p>

      <div className="flex flex-col md:flex-row items-center justify-between gap-4 mt-4">
        <div className="flex flex-wrap items-center gap-2">
          {/* Category */}
          <FilterButton
            data={categoryData}
            loading={loadingCategory}
            isOpen={isOpenCategory}
            setIsOpen={setIsOpenCategory}
            icon={categoryIcon}
            label="دسته‌بندی"
            placeholder="انتخاب دسته‌بندی"
            arrowIcon={arrowDownIcon}
            selectedLabel={selectedCategory?.name ?? null}
            onSelect={(id) =>
              setSelected((prev) => ({
                ...prev,
                categoryId: id,
              }))
            }
          />

          {/* Group */}
          <FilterButton
            data={groupData}
            loading={loadingGroup}
            isOpen={isOpenGroup}
            setIsOpen={setIsOpenGroup}
            icon={groupIcon}
            label="گروه محصول"
            placeholder={
              selected.categoryId ? "انتخاب گروه محصول" : "ابتدا دسته‌بندی را انتخاب کنید"
            }
            arrowIcon={arrowDownIcon}
            selectedLabel={selectedGroup?.name ?? null}
            onSelect={(id) =>
              setSelected((prev) => ({
                ...prev,
                groupId: id,
              }))
            }
            disabled={!selected.categoryId}
          />

          {/* Brand */}
          <FilterButton
            data={brandData}
            loading={loadingBrand}
            isOpen={isOpenBrand}
            setIsOpen={setIsOpenBrand}
            icon={brandIcon}
            label="برند"
            placeholder={
              selected.groupId ? "انتخاب برند" : "ابتدا گروه محصول را انتخاب کنید"
            }
            arrowIcon={arrowDownIcon}
            selectedLabel={selectedBrand?.name ?? null}
            onSelect={(id) =>
              setSelected((prev) => ({
                ...prev,
                brandId: id,
              }))
            }
            disabled={!selected.groupId}
          />
        </div>

        <Link
          to={searchUrl}
          className={`bg-[#1F78AE] text-white py-2 md:px-16 rounded-[10px] cursor-pointer text-center min-w-32 ${
            !canSearch ? "opacity-60 pointer-events-none" : ""
          }`}
        >
          جستجو
        </Link>
      </div>
    </div>
  );
}
