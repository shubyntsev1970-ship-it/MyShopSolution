export type Product = {
    id: number;
    name: string;
    price: number;
    stock: number;
    description?: string | null;
    createdAtUtc: string;
};

export type CreateProductRequest = {
    name: string;
    price: number;
    stock: number;
    description?: string;
};

export type PagedResult<T> = {
    items: T[];
    page: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
};