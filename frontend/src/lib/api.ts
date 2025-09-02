import axios from "axios";

export const baseURL = import.meta.env.VITE_API_BASE_URL || "";

const api = axios.create({
  baseURL, // e.g., https://localhost:7034
  withCredentials: false,
  headers: { "Content-Type": "application/json" },
});

export default api;
