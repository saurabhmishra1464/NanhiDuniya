"use client";
import { signIn, useSession } from 'next-auth/react';
import React, { useState } from 'react'
import { zodResolver } from '@hookform/resolvers/zod';
import { FieldValues, useForm } from 'react-hook-form';
import { LoginFormValidation } from '@/lib/validation';
import { z } from 'zod';
import router, { useRouter } from 'next/navigation';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { toast, ToastContainer } from '@/components/Shared/ToastMessage/ToastContainer';
import LoginError from '@/app/auth/login/error';

const LoginForm = () => {
  const [showPassword, setShowPassword] = useState(false);
  const { data: session } = useSession();
  const [loginError, setLoginError] = useState<{ message: string; statusCode: number } | null>(null);
  const [loading, setLoading] = useState(false);

  const { register, handleSubmit, watch, formState: { errors }, reset } = useForm({
    resolver: zodResolver(LoginFormValidation),
  });
  const router = useRouter();
  const password = watch('password');
  const togglePasswordVisibility = () => {
    setShowPassword(!showPassword);
  };
  const onSubmit = async (data: FieldValues) => {
    setLoading(true);
    try {
      // await new Promise((resolve) => setTimeout(resolve, 5000));
      const response = await signIn("credentials", {
        email: data.userName,
        password: data.password,
        redirect: false,
      });
      if (response?.ok) {
        toast.success("You are now signed in!");
        router.push("/admin/dashboard");
      }
      if (!response?.ok) {
        const errorObject = {
          message: response?.error || 'An unexpected error occurred. Please try again.',
          statusCode: response?.status ?? 0,
        }
        if (response?.status === 401) {
          errorObject.message = 'Invalid email or password';
        } else if (response?.status === 500) {
          errorObject.message = 'Server error, please try again later';
        }
        setLoginError(errorObject);
        toast.error(errorObject.message);
        return;
      }
    }
    catch (error: any) {

      const errorObject = {
        message: error.message || 'An unexpected error occurred. Please try again.',
        statusCode: error.status || 500,
      };

      setLoginError(errorObject);
      toast.error(errorObject.message);
    } finally {
      setLoading(false);
    }
  };
  return (
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
          disabled={loading}
        >
          {loading ? 'Logging In...' : 'Log In'}
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
      {loginError && <LoginError error={loginError} />}
    </form>
  )
}

export default LoginForm