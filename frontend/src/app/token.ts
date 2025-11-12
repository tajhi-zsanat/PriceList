// src/app/token.ts
type TokenCache = {
  accessToken: string | null;
  // optional: token exp if you want to do proactive refresh later
};

const mem: TokenCache = {
  accessToken: null,
};

const LS_KEY = "pl_access_token";

export function setAccessToken(token: string | null) {
  mem.accessToken = token;
  if (token) localStorage.setItem(LS_KEY, token);
  else localStorage.removeItem(LS_KEY);
}

export function getAccessToken(): string | null {
  if (mem.accessToken) return mem.accessToken;
  const fromLs = localStorage.getItem(LS_KEY);
  mem.accessToken = fromLs;
  return fromLs;
}

export function clearAccessToken() {
  setAccessToken(null);
}

// Optional: decode JWT payload (without verifying)
export function decodeJwt<T = any>(token?: string | null): T | null {
  if (!token) return null;
  const parts = token.split(".");
  if (parts.length !== 3) return null;
  try {
    const json = atob(parts[1].replace(/-/g, "+").replace(/_/g, "/"));
    return JSON.parse(decodeURIComponent(escape(json)));
  } catch {
    return null;
  }
}
