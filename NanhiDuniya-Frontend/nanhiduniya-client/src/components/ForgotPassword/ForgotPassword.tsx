'use client'

import React, { useState } from 'react';
import axios from 'axios';

interface ApiResponse<T> {
    success: boolean;
    message: string;
    data: T;
    statusCode: number;
    errors?: any;
}

const ForgotPassword: React.FC = () => {
    const [email, setEmail] = useState<string>('');
    const [loading, setLoading] = useState<boolean>(false);
    const [message, setMessage] = useState<string | null>(null);
    const [error, setError] = useState<string | null>(null);

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setMessage(null);
        setError(null);

        try {
            const response = await axios.post<ApiResponse<object>>(`${process.env.NEXT_PUBLIC_API_URL}/api/Account/forgot-password`, { email });
            if (response.data.success) {
                setMessage('A password reset link has been sent to your email.');
            } else {
                setError(response.data.message);
            }
        } catch (err) {
            console.error('Error sending password reset email:', err);
            setError('An error occurred while sending the password reset email.');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-100">
            <div className="max-w-md w-full bg-white shadow-md rounded-lg p-8">
                <h2 className="text-2xl font-bold text-center mb-6">Forgot Password</h2>
                <form onSubmit={handleSubmit}>
                    <div className="mb-4">
                        <label htmlFor="email" className="block text-gray-700 text-sm font-bold mb-2">
                            Email Address
                        </label>
                        <input
                            type="email"
                            id="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                            className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                        />
                    </div>
                    <div className="flex items-center justify-between">
                        <button
                            type="submit"
                            disabled={loading}
                            className={`bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline ${
                                loading ? 'opacity-50 cursor-not-allowed' : ''
                            }`}
                        >
                            {loading ? 'Sending...' : 'Send Reset Link'}
                        </button>
                    </div>
                </form>
                {message && <div className="mt-4 text-green-500">{message}</div>}
                {error && <div className="mt-4 text-red-500">{error}</div>}
            </div>
        </div>
    );
};

export default ForgotPassword;
