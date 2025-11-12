// src/pages/public/Login.tsx
import { useState, type FormEvent } from "react";
import { useAuth } from "@/app/AuthProvider";
import { useLocation, useNavigate } from "react-router-dom";

export default function Login() {
  const { login } = useAuth();
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [busy, setBusy] = useState(false);
  const [err, setErr] = useState<string | null>(null);

  const navigate = useNavigate();
  const loc = useLocation() as any;
  const redirectTo = loc.state?.from || "/admin";

  const onSubmit = async (e: FormEvent) => {
    e.preventDefault();
    if (busy) return;
    setErr(null);
    setBusy(true);
    try {
      await login({ username, password });
      navigate(redirectTo, { replace: true });
    } catch (ex: any) {
      setErr(ex?.response?.status === 401 ? "نام کاربری یا رمز عبور نادرست است." : "خطا در ورود.");
    } finally {
      setBusy(false);
    }
  };

  return (
    <div className="min-h-[60vh] flex items-center justify-center p-4">
      <form onSubmit={onSubmit} className="w-full max-w-sm space-y-4 border rounded-xl p-6">
        <h1 className="text-xl font-bold text-center">ورود</h1>

        <div className="space-y-1">
          <label className="block text-sm">نام کاربری</label>
          <input
            className="w-full border rounded px-3 py-2"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            autoComplete="username"
            required
          />
        </div>

        <div className="space-y-1">
          <label className="block text-sm">رمز عبور</label>
          <input
            className="w-full border rounded px-3 py-2"
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            autoComplete="current-password"
            required
          />
        </div>

        {err && <div className="text-red-600 text-sm">{err}</div>}

        <button
          type="submit"
          disabled={busy}
          className="w-full rounded-lg px-4 py-2 border hover:bg-gray-50 disabled:opacity-60"
        >
          {busy ? "در حال ورود..." : "ورود"}
        </button>
      </form>
    </div>
  );
}
