'use client';
import { signIn } from 'next-auth/react';
import { redirect } from 'next/dist/server/api-utils';
import router from 'next/router';
import React, { useState } from 'react'

const LoginPage = () => {
  const [userName, setUserName] = useState('');
  const [password, setPassword] = useState('');

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    const res = await signIn('credentials', {
      redirect: false,
      userName,
      password
    });
    if (res && res.ok) {
      router.push('/dashboard')
    } else {
      console.error('Login Failed');
    }
  }

  return (
    <div className="flex flex-col items-center justify-center min-h-screen bg-cover bg-center" style={{ backgroundImage: 'url(/images/LoginBackground.webp)' }}>
      <div className="text-center mb-4">
        <h1 className="text-3xl font-bold text-white">Nanhi Duniya</h1>
      </div>
      <div className="bg-white p-8 rounded-lg shadow-md w-full md:w-96 lg:w-1/2 xl:w-1/3 2xl:w-1/4">
        <h2 className="text-2xl font-semibold mb-4 text-center text-gray-600">Log in to your account</h2>
        <form onSubmit={handleSubmit}>
          <div className="mb-6">
            <input
              type="email"
              id="email"
              className="shadow appearance-none border rounded w-full h-12 py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              value={userName}
              onChange={(e) => setUserName(e.target.value)}
              required
              placeholder="Email Address"
            />
          </div>
          <div className="mb-4">
            <input
              type="password"
              id="password"
              className="shadow appearance-none border rounded w-full h-12 py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
              placeholder="Password"
            />
          </div>
          <div className="flex items-center">
            <button
              type="submit"
              className="bg-red-500 text-white w-full h-12 mt-4 font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
            >
              Log In
            </button>
          </div>
          <div className="mt-4 flex items-center justify-center">
            <a
              href="/forgot-password"
              className="inline-block align-baseline font-bold text-sm text-gray-400 hover:text-blue-800 underline"
            >
              Forgot Password?
            </a>
          </div>
        </form>
        <div className="mt-4 text-center">
          <p className="text-gray-600">Not a provider yet? <a href="/register" className="text-blue-500 hover:text-blue-800">Partner with us</a></p>
        </div>
      </div>
    </div>
  );
}

export default LoginPage