import { useState } from 'react';
import type { SubmitEvent } from 'react';
import { authApi } from '../api/authApi';

export function LoginPage() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const handleSubmit = async (event: SubmitEvent<HTMLFormElement>) => {
        event.preventDefault();

        const result = await authApi.login({ email, password });

        localStorage.setItem('accessToken', result.accessToken);
        localStorage.setItem('refreshToken', result.refreshToken);

        window.location.href = '/products';
    };

    return (
        <form onSubmit={handleSubmit}>
            <h1>Login</h1>

            <input
                value={email}
                onChange={(event) => setEmail(event.target.value)}
                placeholder="Email"
            />

            <input
                value={password}
                onChange={(event) => setPassword(event.target.value)}
                placeholder="Password"
                type="password"
            />

            <button type="submit">Login</button>
        </form>
    );
}
