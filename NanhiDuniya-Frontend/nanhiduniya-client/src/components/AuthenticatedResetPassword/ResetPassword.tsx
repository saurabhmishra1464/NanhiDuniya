'use client'

import React, { useState } from 'react'
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
// import { toast, ToastContainer } from 'react-toastify';
import { FieldValues, useForm } from 'react-hook-form';
import { z } from 'zod';
import { zodResolver } from '@hookform/resolvers/zod';
import { ResetPasswordFormValidation } from '@/lib/validation';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
const ResetPassword = () => {
  const [showPassword, setShowPassword] = useState(false);

  const { register, handleSubmit, formState: { errors }, reset, } = useForm({
    resolver: zodResolver(ResetPasswordFormValidation),
  });
  const togglePasswordVisibility = () => {
    setShowPassword(!showPassword);
  };
  const onSubmit = async (data: FieldValues) => {

  }
  return (
    <div className="flex flex-col items-center justify-center min-h-96 mr-30 bg-gray-100">
    <div className="w-full max-w-md p-8 space-y-6 bg-white shadow-md rounded-lg">

      <h2 className="text-2xl font-bold text-center">Reset Password</h2>
    
      <form onSubmit={handleSubmit(onSubmit)}>
        <div className="mb-4 relative">
          <label htmlFor="newPassword" className="block text-sm font-medium text-gray-700">New Password</label>
          <input
            type={showPassword ? 'text' : 'password'} // Toggle input type
            {...register("newPassword")}
            id="newPassword"
            name="newPassword"
            placeholder="Enter new password"
            className={`mt-1 block w-full px-3 py-2 border ${errors.newPassword ? 'border-red-500' : 'border-gray-300'} rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm`}
          />
          {
            errors?.newPassword && (<span className="text-red-500 text-sm mt-1">{`${errors.newPassword.message}`}</span>)
          }
          <button
            type="button"
            onClick={togglePasswordVisibility}
            className="absolute right-2 top-8 text-gray-600 hover:text-gray-900 focus:outline-none"
          >
            <FontAwesomeIcon icon={showPassword ? faEyeSlash : faEye} size="sm" />
          </button>
        </div>
        <div className="mb-6 relative">
          <label htmlFor="confirmPassword" className="block text-sm font-medium text-gray-700">Confirm Password</label>
          <input
            type={showPassword ? 'text' : 'password'} // Toggle input type
            {...register("confirmPassword")}
            id="confirmPassword"
            name="confirmPassword"
            placeholder="Confirm password"
            className={`mt-1 block w-full px-3 py-2 border ${errors.confirmPassword ? 'border-red-500' : 'border-gray-300'} rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm`}
          />
          {
            errors?.confirmPassword && (<span className='text-red-500 text-sm mt-1'>{`${errors.confirmPassword.message}`}</span>)
          }
          <button
            type="button"
            onClick={togglePasswordVisibility}
            className="absolute right-2 top-8 text-gray-600 hover:text-gray-900 focus:outline-none"
          >
            <FontAwesomeIcon icon={showPassword ? faEyeSlash : faEye} size="sm" />
          </button>
        </div>
        <button type="submit" className="w-full bg-indigo-600 text-white py-2 px-4 rounded-md hover:bg-indigo-700 focus:outline-none focus:bg-indigo-700">
          Reset Password
        </button>
      </form>
      {/* <ToastContainer /> */}
    </div>
  </div>
  )
}

export default ResetPassword