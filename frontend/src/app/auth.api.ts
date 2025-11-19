// src/app/auth.api.ts
import api from "@/lib/api";

export type Role = "Admin" | "Editor" | "User";

export type LoginDto = {
    username: string;
    password: string;
};

export type AuthUser = {
    id: number;
    userName: string;
    displayName: string;
    roles: Role[];
};

export type LoginResponse = {
    accessToken: string;
    user: AuthUser;
};

export async function loginApi(dto: LoginDto): Promise<LoginResponse> {
    const { data } = await api.post<LoginResponse>("/api/Auth/login", dto);
    return data;
}

// If you later add an endpoint like POST /logout to clear refresh cookie on the server:
export async function logoutApi(): Promise<void> {
    try {
        await api.post("/logout");
    } catch {
        // it's fine if server doesn't have it; we'll still clear client state
    }
}

// OPTIONAL: if you expose a /me endpoint to re-hydrate user from server
export async function meApi(): Promise<AuthUser> {
    const { data } = await api.get<AuthUser>("/me");
    return data;
}
