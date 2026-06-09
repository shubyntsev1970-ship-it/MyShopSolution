export type LoginRequest = {
    email: string;
    password: string;
};

export type AuthResponse = {
    accessToken: string;
    refreshToken: string;
    email: string;
    role: string;
};