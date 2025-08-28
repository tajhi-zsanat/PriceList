import { Link, Route, Routes } from "react-router-dom";
import Categories from "./pages/Categories";

export default function App() {
  return (
    <div className="min-h-screen">
      <header className="bg-white border-b sticky top-0 z-10">
        <nav className="max-w-6xl mx-auto px-4 h-14 flex items-center gap-4">
          <Link className="font-bold" to="/">خانه</Link>
          <Link to="/categories">دسته‌بندی‌ها</Link>
        </nav>
      </header>

      <main className="max-w-6xl mx-auto px-4 py-6">
        <Routes>
          <Route path="/" element={<div>به PriceList خوش آمدید</div>} />
          <Route path="/categories" element={<Categories />} />
        </Routes>
      </main>
    </div>
  );
}
