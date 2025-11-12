// src/lib/api.ts
import { getAccessToken } from "@/app/token";
import axios from "axios";

// IMPORTANT: set base URL from env (e.g., VITE_API_BASE_URL="https://localhost:7034")
export const baseURL = import.meta.env.VITE_API_BASE_URL || "";

// We generally want credentials = true so the refresh cookie
// (set during /login) is accepted by the browser in cross-origin scenarios.
// Ensure your server CORS has `AllowCredentials = true` and the Origin whitelisted.
const api = axios.create({
  baseURL,
  withCredentials: true,
  headers: { "Content-Type": "application/json" },
});

// Attach Authorization header if we have a token
api.interceptors.request.use((config) => {
  const token = getAccessToken();
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

export default api;
