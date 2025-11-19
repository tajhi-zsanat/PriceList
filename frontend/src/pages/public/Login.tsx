// src/pages/public/Login.tsx
import { useState, type FormEvent } from "react";
import { useAuth } from "@/app/AuthProvider";
import { Link, useLocation, useNavigate } from "react-router-dom";
import vector from "@/assets/img/vector.png";
import BackGroundImage from "@/assets/img/BackGroundImage.png";
import pikatak from "@/assets/img/pikatak.png";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";

export default function Login() {
  const { login } = useAuth();

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [busy, setBusy] = useState(false);
  const [err, setErr] = useState<string | null>(null);
  const [showPass, setShowPass] = useState(false);

  const navigate = useNavigate();
  const loc = useLocation() as any;
  const redirectTo = loc.state?.from || "/admin/forms";

  const onSubmit = async (e: FormEvent) => {
    e.preventDefault();
    if (busy) return;
    setErr(null);
    setBusy(true);

    try {
      await login({ username, password });
      navigate(redirectTo, { replace: true });
    } catch (ex: any) {
      setErr(
        ex?.response?.status === 401
          ? "نام کاربری یا رمز عبور نادرست است."
          : "خطا در ورود."
      );
    } finally {
      setBusy(false);
    }
  };

  return (
    <div className="flex flex-col md:flex-row items-center justify-between flex-1 pt-4">

      <div className="m-auto w-full max-w-md p-4">
        <Link
          to="/"
          className="flex items-center gap-2 hover:text-[#1F78AE] transition duration-200 text-inherit"
        >
          <i className="fa-solid fa-arrow-right text-inherit"></i>
          <span className="text-inherit">بازگشت</span>
        </Link>
        <div className="mb-5 text-center md:text-left mt-8">
          <img src={pikatak} className="mb-3 mx-auto md:mx-0" width="150" />
        </div>

        <p className="text-center text-base font-medium mb-4">
          لطفاً اطلاعات حساب کاربری خود را وارد کنید
        </p>

        {err && (
          <p className="text-red-500 text-center mb-3 text-sm">{err}</p>
        )}

        <form onSubmit={onSubmit} className="mt-4">
          <div className="flex flex-col gap-8">

            {/* USERNAME */}
            <div className="relative">
              <Input
                id="username"
                value={username}
                onChange={e => setUsername(e.target.value)}
                className="w-full border rounded-xl px-5 py-5 focus:outline-none focus:ring-2 focus:ring-primary"
                placeholder="نام کاربری"
              />
              <i className="fas fa-user absolute left-3 top-1/2 -translate-y-1/2 text-gray-400"></i>
            </div>

            {/* PASSWORD */}
            <div className="relative">
              <Input
                id="password"
                type={showPass ? "text" : "password"}
                value={password}
                onChange={e => setPassword(e.target.value)}
                className="w-full border rounded-xl px-5 pr-10 py-5 focus:outline-none focus:ring-2 focus:ring-primary"
                placeholder="رمز عبور"
              />

              <i className="fas fa-lock absolute left-3 top-1/2 -translate-y-1/2 text-gray-400"></i>

              <i
                onClick={() => setShowPass(p => !p)}
                className={`fas ${showPass ? "fa-eye-slash" : "fa-eye"
                  } absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 cursor-pointer`}
              ></i>
            </div>
          </div>

          {/* LOGIN BUTTON */}
          <Button
            type="submit"
            disabled={busy}
            className="bg-[#1F78AE] w-full mt-8 py-5 text-white rounded-xl"
          >
            {busy ? "صبور باشید..." : "ورود"}
          </Button>
        </form>
      </div>

      {/* RIGHT SIDE IMAGES */}
      <div className="hidden md:block">
        <div className="relative">
          <img src={BackGroundImage} aria-hidden="true" />
          <img src={vector} className="absolute bottom-24 -right-4" aria-hidden="true" />
        </div>
      </div>
    </div>
  );
}
