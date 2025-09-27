import { Routes, Route, Navigate } from "react-router-dom";
import Category from "./pages/category/Category";
import ProductGroups from "./pages/productGroups/ProductGroups";
import ProductTypes from "./pages/productTypes/ProductTypes";
import Brands from "./pages/brands/Brands";
import Products from "./pages/products/Products";
import Header from "./Header";

export default function App() {
  return (
    <div className="flex flex-col min-h-screen">
      <Header />
      <main className="flex flex-1 ">
        <Routes>
          <Route path="/" element={<Navigate to="/Category" replace />} />
          <Route path="/Category" element={<Category />} />
          <Route path="/Category/:categoryId/groups" element={<ProductGroups />} />
          <Route path="/Category/:categoryId/groups/:groupId/types" element={<ProductTypes />} />
          <Route path="/Category/:categoryId/groups/:groupId/types/:typeId/brands" element={<Brands />} />
          <Route path="/Category/:categoryId/groups/:groupId/types/:typeId/brands/:brandId/products" element={<Products />} />
          <Route path="*" element={<div className="p-6">یافت نشد</div>} />
        </Routes>
      </main>
    </div>
  );
}
