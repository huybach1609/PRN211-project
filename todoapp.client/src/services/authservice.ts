import { AxiosResponse } from "axios";
import { createApiInstance } from "../utils/apiUtils";
import { AuthResponse } from "../types/User";

const api = createApiInstance();

export interface LoginRequest {
    username?: string;
    password?: string;
}
export const AuthService = {
    login: async (request: LoginRequest): Promise<AxiosResponse<AuthResponse>> => {
        return api.post<AuthResponse>("/auth/login", request);
    }
}

