'use client'
import { ApiResponse } from '@/model/Responses/ApiResponse';
import { handleError } from '@/utils/ErrorHandelling/errorHandler';
import axios from 'axios';
import Head from 'next/head';
import { useRouter, useSearchParams } from 'next/navigation';
import { toast } from 'react-toastify';

export default function ConfirmEmail() {
  debugger
  const searchParams = useSearchParams();
  const email = searchParams.get('email');
  const resendEmail = async () => {
    try {
      const response = await axios.post<ApiResponse<object>>(`${process.env.NEXT_PUBLIC_API_URL}/api/Account/SendConfirmationEmail`, { email: email });
      if (response.data?.success) {
        toast.success("Email Sent Successfully");
      } else {
        const errorMessage = response.data?.message || "Failed to verify email";
        if (response.status === 404) {
          toast.error("User does not exist");
        } else if (response.status === 409) {
          toast.error(errorMessage);
        } else {
          toast.error(errorMessage);
        }
      }
    } catch (error: any) {
      console.log(error);
      let message = handleError(error);
      toast.error(message || "Failed To send Email");
    }
  };
  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-100">
      <Head>
        <title>Confirm Email</title>
      </Head>
      <div className="bg-white p-8 rounded-lg shadow-lg max-w-md w-full">
        <h1 className="text-2xl font-bold mb-4 text-center">Confirm Your Email</h1>
        <p className="mb-6 text-center text-gray-600">
          We have sent a confirmation email to your inbox. Please check your email and click on the confirmation link to verify your account.
        </p>
        <div className="flex justify-center">
          <button className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 transition duration-200" onClick={() => resendEmail()}>
            Resend Email
          </button>
        </div>
      </div>
    </div>
  );
}
