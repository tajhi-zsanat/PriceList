// src/lib/api/auth.ts
import { http, setAccessToken } from "./http";

export async function login(userName: string, password: string) {
  const { data } = await http.post("/auth/login", { userName, password });
  setAccessToken(data.accessToken);
  return data.user; // { id, userName, displayName, roles }
}

export async function logout() {
  await http.post("/auth/logout");
  setAccessToken(null);
}

export async function register(body: { userName: string; email: string; displayName: string; password: string; }) {
  await http.post("/auth/register", body);
}
