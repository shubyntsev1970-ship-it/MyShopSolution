import { BrowserRouter, Link, Route, Routes } from 'react-router-dom';
import { LoginPage } from './pages/LoginPage';
import { ProductsPage } from './pages/ProductsPage';

export function App() {
    return (
        <BrowserRouter>
            <nav>
                <Link to="/login">Login</Link>{' '}
                <Link to="/products">Products</Link>
            </nav>

            <Routes>
                <Route path="/login" element={<LoginPage />} />
                <Route path="/products" element={<ProductsPage />} />
            </Routes>
        </BrowserRouter>
    );
}