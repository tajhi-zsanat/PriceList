// src/lib/api.ts
import { clearAccessToken, getAccessToken, setAccessToken } from "@/app/token";
import axios, { AxiosError, type InternalAxiosRequestConfig } from "axios";

const LS_USER_KEY = "pl_user_key";

function clearStoredUser() {
  if (typeof window !== "undefined") {
    localStorage.removeItem(LS_USER_KEY);
  }
}

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

api.interceptors.request.use((config) => {
  const token = getAccessToken();
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

let isRefreshing = false;
let pendingRequests: ((token: string | null) => void)[] = [];

const refreshClient = axios.create({
  baseURL: baseURL,
  withCredentials: true,
});

function subscribeTokenRefresh(cb: (token: string | null) => void) {
  pendingRequests.push(cb);
}

function onRefreshed(newToken: string | null) {
  pendingRequests.forEach((cb) => cb(newToken));
  pendingRequests = [];
}

api.interceptors.response.use(
  (response) => response,
  async (error: AxiosError) => {
    const status = error.response?.status;
    const originalRequest = error.config as InternalAxiosRequestConfig & {
      _retry?: boolean;
    };

    if (!status) return Promise.reject(error);

    const url = originalRequest.url ?? "";

    // Do NOT try refresh for auth endpoints themselves
    if (
      url.includes("/api/Auth/login") ||
      url.includes("/api/Auth/register") ||
      url.includes("/api/Auth/refresh")
    ) {
      return Promise.reject(error);
    }

    // Only handle first 401 for this request
    if (status !== 401 || originalRequest._retry) {
      return Promise.reject(error);
    }

    // No access token = not logged in → redirect
    if (!getAccessToken()) {
      clearAccessToken();
      clearStoredUser();
      if (!window.location.pathname.startsWith("/login")) {
        window.location.href = "/login";
      }
      return Promise.reject(error);
    }

    originalRequest._retry = true;

    // 🔹 FIRST request that hits 401: start refresh
    if (!isRefreshing) {
      isRefreshing = true;

      try {
        const { data } = await refreshClient.post<{ accessToken: string }>(
          "/api/Auth/refresh",
          null,
          {
            headers: {
              "X-PL-Refresh-CSRF": "1",
            },
          }
        );

        const newToken = data.accessToken;

        // Save new token
        setAccessToken(newToken);

        // Wake up all queued requests
        onRefreshed(newToken);

        // This FIRST request retries immediately
        originalRequest.headers = originalRequest.headers ?? {};
        originalRequest.headers.Authorization = `Bearer ${newToken}`;

        return api(originalRequest);
      } catch (e) {
        // Refresh failed: clear everything, notify queued requests, redirect
        onRefreshed(null);
        clearAccessToken();
        clearStoredUser();

        if (!window.location.pathname.startsWith("/login")) {
          window.location.href = "/login";
        }

        isRefreshing = false;
        return Promise.reject(e);
      } finally {
        isRefreshing = false;
      }
    }

    return new Promise((resolve, reject) => {
      subscribeTokenRefresh((newToken) => {
        if (!newToken) {
          reject(error);
          return;
        }

        originalRequest.headers = originalRequest.headers ?? {};
        originalRequest.headers.Authorization = `Bearer ${newToken}`;

        resolve(api(originalRequest));
      });
    });
  }
);

export default api;
