import axios from "axios";
import { baseURL } from "../api";

let accessToken: string | null = null;
export const setAccessToken = (t: string | null) => (accessToken = t);

export const http = axios.create({
  baseURL: baseURL,
  withCredentials: true
});

http.interceptors.request.use((config) => {
  if (accessToken) config.headers.Authorization = `Bearer ${accessToken}`;
  return config;
});

let refreshing = false;
let queue: Array<() => void> = [];

http.interceptors.response.use(
  r => r,
  async error => {
    const original = error.config;

    if (error.response?.status === 401 && !original._retry) {
      if (refreshing) {
        // wait
        await new Promise<void>(res => queue.push(res));
      } else {
        refreshing = true;
        original._retry = true;
        try {
          const { data } = await http.post("/auth/refresh");
          setAccessToken(data.accessToken);
          queue.forEach(fn => fn());
        } catch {
          setAccessToken(null);
          queue = [];
          // optional: redirect to /login
        } finally {
          refreshing = false;
          queue = [];
        }
      }
      return http(original);
    }

    return Promise.reject(error);
  }
);
