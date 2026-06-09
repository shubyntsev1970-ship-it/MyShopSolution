import { httpClient } from './httpClient';
import type { AuthResponse, LoginRequest } from '../types/auth';

export const authApi = {
    login: async (request: LoginRequest): Promise<AuthResponse> => {
        const response = await httpClient.post<AuthResponse>('/auth/login', request);
        return response.data;
    },

    logout: async (): Promise<void> => {
        const refreshToken = localStorage.getItem('refreshToken');

        if (refreshToken) {
            await httpClient.post('/auth/logout', { refreshToken });
        }

        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
    },
};