"use client"

import { logoutAccount } from '@/lib/actions/user.actions';
import { axiosPrivate } from '@/utils/AxiosInstances/api';
// import { handleError } from '@/utils/ErrorHandelling/errorHandler';
import { signOut, useSession } from 'next-auth/react';
import { useRouter } from 'next/navigation'
import React from 'react';
import { toast } from 'react-toastify';

type LogoutModalProps = {
    // isOpen: boolean;
    onClose: () => void;
    // onError: (message: string) => void;
    logout: ()=>void;
}
// const LogoutModal = ({ isOpen, onClose, onError }: LogoutModalProps) => {
  const LogoutModal = ({ onClose,logout }: LogoutModalProps) => {
    const router = useRouter();
    // if (!isOpen) return null;
    // const logOut = async () => {
    //     try {
    //         const userId = session?.user?._id;
    //         await axiosInstance.post('/api/Account/RevokeRefreshToken', { userId });
    //         await signOut({ callbackUrl: '/auth/login' });
    //     } catch (error) {
    //         // const message = handleError(error as Error);
    //         // onError(message);
    //     }
    //     finally {
    //         onClose();
    //     }
    // }

    // const handleLogout = async ()=>{
    //   debugger
    //   const loggedOut = await logoutAccount();
    //   // await axiosPrivate.post('/api/Account/RevokeRefreshToken', { userId: userId});
    //   console.log(loggedOut);
    //   if(loggedOut) router.push('/auth/login');
    // }

    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
        <div className="bg-white p-6 rounded-md shadow-lg max-w-sm w-full">
          <h2 className="text-lg font-bold mb-4">Are you sure you want to log out?</h2>
          <div className="flex justify-end gap-2">
            <button
              onClick={onClose}
              className="bg-gray-200 hover:bg-gray-300 text-gray-700 font-bold py-2 px-4 rounded"
            >
              Cancel
            </button>
            <button
              onClick={logout} // Assuming `onError` is a function for logging out
              className="bg-red-500 hover:bg-red-700 text-white font-bold py-2 px-4 rounded"
            >
              Log Out
            </button>
          </div>
        </div>
      </div>
    )
}

export default LogoutModal