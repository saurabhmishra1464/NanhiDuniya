'use client';
import { signIn } from 'next-auth/react';
import { redirect } from 'next/dist/server/api-utils';
import router from 'next/router';
import React, { useState } from 'react'
import { zodResolver } from '@hookform/resolvers/zod';
import { FieldValues, useForm } from 'react-hook-form';
import { LoginFormValidation } from '@/lib/validation';
import { z } from 'zod';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { LoginDto } from '../../../model/User';

const LoginPage = () => {
  const [showPassword, setShowPassword] = useState(false);

  const { register, handleSubmit, watch, formState: { errors }, reset } = useForm({
    resolver: zodResolver(LoginFormValidation),
  });

  const password = watch('password');

  const onSubmit = async (data: FieldValues) => {
    debugger
    const login: LoginDto = { email: data.userName, password: data.password };
    //   const res = await signIn('credentials', {
    //     redirect: false,
    //     userName,
    //     password
    //   });
    //   if (res && res.ok) {
    //     router.push('/dashboard')
    //   } else {
    //     console.error('Login Failed');
    //   }
    // }
  }
    const togglePasswordVisibility = () => {
      setShowPassword(!showPassword);
    };

    return (
      <div className="flex flex-col items-center justify-center min-h-screen bg-cover bg-center" style={{ backgroundImage: 'url(/images/LoginBackground.webp)' }}>
        <div className="text-center mb-4">
          <h1 className="text-3xl font-bold text-white">Nanhi Duniya</h1>
        </div>
        <div className="bg-white p-8 rounded-lg shadow-md w-full md:w-96 lg:w-1/2 xl:w-1/3 2xl:w-1/4">
          <h2 className="text-2xl font-semibold mb-4 text-center text-gray-600">Log in to your account</h2>
          <form onSubmit={handleSubmit(onSubmit)}>
            <div className="mb-6 relative">
              <input
                type="email"
                {...register("userName")}
                id="userName"
                name='userName'
                placeholder="Email Address"
                className={`shadow appearance-none border rounded w-full h-12 py-2 px-3 ${errors.username ? 'border-red-500' : 'border-gray-300'} text-gray-700 leading-tight focus:outline-none focus:shadow-outline`}
              />
              {
                errors?.userName && (<span className="text-red-500 text-sm mt-1">{`${errors.userName.message}`}</span>)
              }
            </div>
            <div className="mb-4 relative">
              <input
                type={showPassword ? 'text' : 'password'}
                {...register("password")}
                id="password"
                name='password'
                placeholder="Password"
                className="shadow appearance-none border rounded w-full h-12 py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
              // onChange={(e)=>{
              //   setPassword(e.target.value)
              // }}
              />
              {
                errors?.password && (<span className='text-red-500 text-sm mt-1'>{`${errors.password.message}`}</span>)
              }
              {password && (
                <button
                  type="button"
                  onClick={togglePasswordVisibility}
                  className="absolute right-2 top-3.5 text-gray-600 hover:text-gray-900 focus:outline-none"
                >
                  <FontAwesomeIcon icon={showPassword ? faEyeSlash : faEye} size="sm" />
                </button>
              )}
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
        </div>
      </div>
    );
  }

export default LoginPage