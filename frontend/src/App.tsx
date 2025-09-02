import { Routes, Route, Navigate } from "react-router-dom";
import Categories from "./pages/Categories";
import ProductGroups from "./pages/ProductGroups";
import ProductTypes from "./pages/ProductTypes";
import Brands from "./pages/Brands";
import Products from "./pages/Products";
import Header from "./Header";

export default function App() {
  return (
    <div className="flex flex-col min-h-screen">
      <Header />
      <main className="flex-1 bg-[#F5F5F5]">
        <Routes>
          <Route path="/" element={<Navigate to="/categories" replace />} />
          <Route path="/categories" element={<Categories />} />
          <Route path="/categories/:categoryId/groups" element={<ProductGroups />} />
          <Route path="/categories/:categoryId/groups/:groupId/types" element={<ProductTypes />} />
          <Route path="/categories/:categoryId/groups/:groupId/types/:typeId/brands" element={<Brands />} />
          <Route path="/categories/:categoryId/groups/:groupId/types/:typeId/brands/:brandId/products" element={<Products />} />
          <Route path="*" element={<div className="p-6">یافت نشد</div>} />
        </Routes>
      </main>
    </div>
  );
}
