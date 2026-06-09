import { useEffect, useState } from 'react';
import type { SubmitEvent } from 'react';
import { productsApi } from '../api/productsApi';
import type { Product } from '../types/product';

export function ProductsPage() {
    const [products, setProducts] = useState<Product[]>([]);
    const [search, setSearch] = useState('');
    const [appliedSearch, setAppliedSearch] = useState('');
    const [page, setPage] = useState(1);
    const [refreshKey, setRefreshKey] = useState(0);
    const [name, setName] = useState('');
    const [price, setPrice] = useState(0);
    const [stock, setStock] = useState(0);

    useEffect(() => {
        let ignore = false;

        void productsApi.getProducts({
            page,
            pageSize: 10,
            search: appliedSearch,
            sortBy: 'id',
            sortDirection: 'asc',
        }).then((result) => {
            if (!ignore) {
                setProducts(result.items);
            }
        });

        return () => {
            ignore = true;
        };
    }, [appliedSearch, page, refreshKey]);

    const handleSearch = () => {
        setAppliedSearch(search);
        setPage(1);
        setRefreshKey((current) => current + 1);
    };

    const handleCreate = async (event: SubmitEvent<HTMLFormElement>) => {
        event.preventDefault();

        await productsApi.createProduct({
            name,
            price,
            stock,
        });

        setName('');
        setPrice(0);
        setStock(0);
        setRefreshKey((current) => current + 1);
    };

    const handleDelete = async (id: number) => {
        await productsApi.deleteProduct(id);
        setRefreshKey((current) => current + 1);
    };

    return (
        <div>
            <h1>Products</h1>

            <div>
                <input
                    value={search}
                    onChange={(event) => setSearch(event.target.value)}
                    placeholder="Search products"
                />
                <button onClick={handleSearch}>Search</button>
            </div>

            <form onSubmit={handleCreate}>
                <input
                    value={name}
                    onChange={(event) => setName(event.target.value)}
                    placeholder="Name"
                />

                <input
                    value={price}
                    onChange={(event) => setPrice(Number(event.target.value))}
                    type="number"
                    placeholder="Price"
                />

                <input
                    value={stock}
                    onChange={(event) => setStock(Number(event.target.value))}
                    type="number"
                    placeholder="Stock"
                />

                <button type="submit">Create product</button>
            </form>

            <table>
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Name</th>
                        <th>Price</th>
                        <th>Stock</th>
                        <th>Actions</th>
                    </tr>
                </thead>

                <tbody>
                    {products.map((product) => (
                        <tr key={product.id}>
                            <td>{product.id}</td>
                            <td>{product.name}</td>
                            <td>{product.price}</td>
                            <td>{product.stock}</td>
                            <td>
                                <button onClick={() => handleDelete(product.id)}>Delete</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            <button disabled={page === 1} onClick={() => setPage(page - 1)}>
                Prev
            </button>

            <button onClick={() => setPage(page + 1)}>
                Next
            </button>
        </div>
    );

}
