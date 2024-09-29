
'use client'
import axios from 'axios';
import Link from 'next/link';
import { useSearchParams } from 'next/navigation';
import React, { useEffect, useState } from 'react';

const VerifyEmailPage = () => {
    const searchParams = useSearchParams();
    const [loading, setLoading] = useState(true);
    const [message, setMessage] = useState({ type: '', text: '' });

    useEffect(() => {
        debugger
        const token = searchParams?.get('token') || '';
        const email = searchParams?.get('email') || '';

        const verifyEmail = async () => {
            debugger
            if (!token) {
                setMessage({ type: 'error', text: 'Email verification token not found!' });
                setLoading(false);
                return;
            }

            try {
                const response = await axios.get(`${process.env.NEXT_PUBLIC_API_URL}/api/Account/Verify-Email`, {
                    params: { token, email },
                });

                if (response.data.statusCode===200) {
                    setMessage({ type: 'success', text: 'Email verified successfully!' });
                } else {
                    setMessage({ type: 'error', text: 'Failed to verify email.' });
                }

            } catch (error) {
                setMessage({ type: 'error', text: 'Error occurred during email verification.' });
                console.error('Error verifying email:', error);
            } finally {
                setLoading(false);
            }
        };

        verifyEmail();
    }, [searchParams]);

    // Display loading message while verification is in progress
    if (loading) {
        return <div>Loading...</div>;
    }

    return (
        <div className="container px-4 py-16 text-center">
            {message.type === 'success' ? (
                <>
                    <h1 className="text-3xl font-bold mb-4">Email Verification Successful!</h1>
                    <p className="text-lg mb-8">{message.text}</p>
                </>
            ) : (
                <div className="text-red-500">
                    <h1 className="text-3xl font-bold mb-4">Verification Failed</h1>
                    <p className="text-lg mb-8">{message.text}</p>
                </div>
            )}

            <div className="flex justify-center">
                <Link href="/" className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">
                    Go to Homepage
                </Link>
            </div>
        </div>
    );
};

export default VerifyEmailPage;