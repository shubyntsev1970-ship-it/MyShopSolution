import { httpClient } from './httpClient';
import type { CreateProductRequest, PagedResult, Product } from '../types/product';

export const productsApi = {
    getProducts: async (params: {
        page: number;
        pageSize: number;
        search?: string;
        sortBy?: string;
        sortDirection?: string;
    }): Promise<PagedResult<Product>> => {
        const response = await httpClient.get<PagedResult<Product>>('/products', { params });
        return response.data;
    },

    createProduct: async (request: CreateProductRequest): Promise<Product> => {
        const response = await httpClient.post<Product>('/products', request);
        return response.data;
    },

    updateProduct: async (id: number, request: CreateProductRequest): Promise<Product> => {
        const response = await httpClient.put<Product>(`/products/${id}`, request);
        return response.data;
    },

    patchProduct: async (id: number, request: Partial<CreateProductRequest>): Promise<Product> => {
        const response = await httpClient.patch<Product>(`/products/${id}`, request);
        return response.data;
    },

    deleteProduct: async (id: number): Promise<void> => {
        await httpClient.delete(`/products/${id}`);
    },
};