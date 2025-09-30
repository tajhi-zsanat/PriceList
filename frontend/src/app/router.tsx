// src/app/router.tsx
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import { Suspense, lazy } from "react";
import PublicLayout from "@/layouts/PublicLayout";
import AdminLayout from "@/layouts/AdminLayout";
import Protected from "./guards/Protected";
import RoleGuard from "./guards/RoleGuard";

// Public pages
const Category = lazy(() => import("@/pages/public/category/Category"));
const ProductGroups = lazy(() => import("@/pages/public/productGroups/ProductGroups"));
const ProductTypes = lazy(() => import("@/pages/public/productTypes/ProductTypes"));
const Brands = lazy(() => import("@/pages/public/brands/Brands"));
const ProductsBySupplier = lazy(() => import("@/pages/public/products/ProductsBySupplier"));
const Login = lazy(() => import("@/pages/public/Login"));

// Admin pages
const Dashboard = lazy(() => import("@/pages/admin/dashboard/Dashboard"));
const AdminProducts = lazy(() => import("@/pages/admin/products/Products"));
const Forms = lazy(() => import("@/pages/admin/forms/Forms"));

const Forbidden = () => <div>403 — دسترسی ندارید</div>;
const NotFound = () => <div>404</div>;

const Load = (el: React.ReactElement) => <Suspense fallback={null}>{el}</Suspense>;

const router = createBrowserRouter([
  {
    element: <PublicLayout />,
    children: [
      { index: true, element: Load(<Category />) }, // instead of path: "/"
      { path: "/Category/:categoryId/groups", element: Load(<ProductGroups />) },
      { path: "/Category/:categoryId/groups/:groupId/types", element: Load(<ProductTypes />) },
      { path: "/Category/:categoryId/groups/:groupId/types/:typeId/brands", element: Load(<Brands />) },
      { path: "/Category/:categoryId/groups/:groupId/types/:typeId/brands/:brandId/products", element: Load(<ProductsBySupplier />) },
      { path: "/login", element: <Login /> },
      { path: "/403", element: <Forbidden /> },
    ],
  },
  {
    path: "/admin",
    //element: <Protected />,
    children: [
      {
        //element: <RoleGuard roles={["Admin", "Editor"]} />,
        children: [
          {
            element: <AdminLayout />,
            children: [
              { index: true, element: Load(<Dashboard />) },
              { path: "forms", element: Load(<Forms />) },
              { path: "products", element: Load(<AdminProducts />) },
            ],
          },
        ],
      },
    ],
  },
  { path: "*", element: <NotFound /> },
]);

export default function AppRouter() {
  return <RouterProvider router={router} />;
}
