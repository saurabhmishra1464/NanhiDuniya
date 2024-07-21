"use client"
import { useRouter, useSearchParams } from 'next/navigation';
import { useState, useEffect } from 'react';
import Link from 'next/link';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import { toast, ToastContainer } from '../../../components/Shared/ToastMessage/ToastContainer';
// import { toast, ToastContainer } from 'react-toastify';
import UserService from '@/services/UserManagement/UserService';
import { ResetPassword } from '@/model/User';
import { FieldValues, useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { ResetPasswordFormValidation } from '@/lib/validation';
import axios from 'axios';

const ResetPasswordPage = () => {
  const searchParams = useSearchParams();
  const token = searchParams?.get('token') || '';
  const email = searchParams?.get('email') || '';
  const router = useRouter();
  const [success, setSuccess] = useState('');
  const [ShowNewPassword, setShowNewPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);

  const { register, handleSubmit, watch, formState: { errors }, reset } = useForm({
    resolver: zodResolver(ResetPasswordFormValidation),
  });

  const newPassword = watch('newPassword');
  const confirmPassword = watch('confirmPassword');

  useEffect(() => {
    if (!token || !email) {
      toast.error('Invalid password reset link.');
    }
  }, [token, email]);

  const onSubmit = async (data: FieldValues) => {

    if (data.newPassword !== data.confirmPassword) {
      toast.error('Passwords do not match.');
      return;
    }
    const resetPasswordData: ResetPassword = { email, token, newPassword: data.newPassword };

    try {
      const response = await UserService.resetPassword(resetPasswordData);
      if (response?.statusCode === 200) {

        toast.success('Password reset successful.');
        reset();
      } else {

        toast.error(response?.message || 'Error resetting password.');
      }
    } catch (error) {

      if (axios.isAxiosError(error) && error.response) {
        toast.error(error.response.data.message);
      }
      else {
        toast.error('An unexpected error occurred');
      }
    }
  };

  const togglePasswordVisibility = (field: 'newPassword' | 'confirmPassword') => {
    if (field === 'newPassword') {
      setShowNewPassword(prev => !prev)
    } else if (field === 'confirmPassword') {
      setShowConfirmPassword(prev => !prev);
    }
  };



  return (
    <div className="flex items-center justify-center min-h-screen bg-gray-100">
      <div className="w-full max-w-md p-8 space-y-6 bg-white shadow-md rounded-lg">
        <h2 className="text-2xl font-bold text-center">Reset Password</h2>
        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="mb-4 relative">
            <label htmlFor="newPassword" className="block text-sm font-medium text-gray-700">New Password</label>
            <input
              type={ShowNewPassword ? 'text' : 'password'} // Toggle input type
              {...register("newPassword")}
              id="newPassword"
              name="newPassword"
              placeholder="Enter new password"
              className={`mt-1 block w-full px-3 py-2 border ${errors.newPassword ? 'border-red-500' : 'border-gray-300'} rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm`}
            />
            {
              errors?.newPassword && (<span className="text-red-500 text-sm mt-1">{`${errors.newPassword.message}`}</span>)
            }
            {newPassword && (
              <button
                type="button"
                onClick={() => { togglePasswordVisibility('newPassword') }}
                className="absolute right-2 top-8 text-gray-600 hover:text-gray-900 focus:outline-none"
              >
                <FontAwesomeIcon icon={ShowNewPassword ? faEyeSlash : faEye} size="sm" />
              </button>
            )}

          </div>
          <div className="mb-6 relative">
            <label htmlFor="confirmPassword" className="block text-sm font-medium text-gray-700">Confirm Password</label>
            <input
              type={showConfirmPassword ? 'text' : 'password'} // Toggle input type
              {...register("confirmPassword")}
              id="confirmPassword"
              name="confirmPassword"
              placeholder="Confirm password"
              className={`mt-1 block w-full px-3 py-2 border ${errors.confirmPassword ? 'border-red-500' : 'border-gray-300'} rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm`}
            />
            {
              errors?.confirmPassword && (<span className='text-red-500 text-sm mt-1'>{`${errors.confirmPassword.message}`}</span>)
            }
            {confirmPassword && (
              <button
                type="button"
                onClick={() => { togglePasswordVisibility('confirmPassword') }}
                className="absolute right-2 top-8 text-gray-600 hover:text-gray-900 focus:outline-none"
              >
                <FontAwesomeIcon icon={showConfirmPassword ? faEyeSlash : faEye} size="sm" />
              </button>
            )}
          </div>
          <button type="submit" className="w-full bg-indigo-600 text-white py-2 px-4 rounded-md hover:bg-indigo-700 focus:outline-none focus:bg-indigo-700">
            Reset Password
          </button>
        </form>
        <ToastContainer />
      </div>
    </div>
  );
}

export default ResetPasswordPage