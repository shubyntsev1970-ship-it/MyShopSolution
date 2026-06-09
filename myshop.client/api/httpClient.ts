import axios from 'axios';

export const httpClient = axios.create({
    baseURL: 'https://localhost:5001/api',
});

// Перед каждым запросом добавляем access token.
httpClient.interceptors.request.use((config) => {
    const accessToken = localStorage.getItem('accessToken');

    if (accessToken) {
        config.headers.Authorization = `Bearer ${accessToken}`;
    }

    return config;
});

// Если access token умер, пробуем refresh.
httpClient.interceptors.response.use(
    (response) => response,
    async (error) => {
        const originalRequest = error.config;

        if (error.response?.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true;

            const refreshToken = localStorage.getItem('refreshToken');

            if (!refreshToken) {
                throw error;
            }

            const response = await axios.post('https://localhost:5001/api/auth/refresh', {
                refreshToken,
            });

            localStorage.setItem('accessToken', response.data.accessToken);
            localStorage.setItem('refreshToken', response.data.refreshToken);

            originalRequest.headers.Authorization = `Bearer ${response.data.accessToken}`;

            return httpClient(originalRequest);
        }

        throw error;
    }
);