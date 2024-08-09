'use client';

import Link from 'next/link';
import React, { useState } from 'react';
import { UserCircleIcon, Cog6ToothIcon, QuestionMarkCircleIcon, XMarkIcon } from '@heroicons/react/24/outline';
import { signOut, useSession } from 'next-auth/react';
import axiosInstance from '@/utils/AxiosInstances/api';
import { handleError } from '@/utils/ErrorHandelling/errorHandler';
import LogoutModal from '../modals/LogoutModal';
import { toast } from 'react-toastify';

export default function Navbar() {
  const [isOpen, setIsOpen] = useState<boolean>(false);
  const [isLogoutModalOpen, setIsLogoutModalOpen] = useState(false);

  const handleClick = () => {
    setIsOpen(!isOpen);
  }

  const handleLogoutClick = () => {
    setIsLogoutModalOpen(true);
  }
  const handleModalClose = () => {
    setIsLogoutModalOpen(false);
  };

  const handleLogoutError = (errorMessage: string) => {
    toast.error(errorMessage);
  }

  return (
    <nav className='bg-gray-800 p-0 relative'>
      <div className='flex justify-between items-center'>
        <div className='flex items-center text-white text-2xl font-semibold'>
          <Link href='/'>
            <img src="/assets/images/Logo.png" alt="NanhiDuniya Logo" className="h-16 mx-2 object-contain" />
          </Link>
          <p className="hidden xl:block text-lg font-semibold ml-12">NanhiDuniya (Growing Smiles Through Joyful Education)</p>
        </div>
        <div className="relative md:mr-0 lg:mr-4">
          <div onClick={handleClick} className="bg-gray-800 sm:p-0 md:p-0 xl:p-4 rounded-lg shadow-lg hover:bg-gray-700 transition-colors duration-300">
            <div className="flex items-center">
              <img src='/assets/images/profilePic.jpg' alt="Profile picture" className="w-12 h-12 rounded-full mr-4" />
              <div className="hidden xl:block lg:block">
                <span className="text-sm text-gray-200">Saurabh Mishra</span>
                <div className="text-xs text-gray-400">Admin</div>
              </div>
            </div>
          </div>
          {isOpen && (
            <ul className="absolute right-0 mt-2 w-48 bg-gray-800 rounded-lg shadow-lg overflow-hidden">
              <li className="flex items-center text-white text-sm hover:bg-gray-700 pl-6">
                <UserCircleIcon className="w-5 h-5 mr-3 text-gray-400" />
                <a href="#" className="block px-4 py-2">My Account</a>
              </li>
              <li className="flex items-center text-white text-sm hover:bg-gray-700 pl-6">
                <Cog6ToothIcon className="w-5 h-5 mr-3 text-gray-400" />
                <a href="#" className="block px-4 py-2">Settings</a>
              </li>
              <li className="flex items-center text-white text-sm hover:bg-gray-700 pl-6">
                <QuestionMarkCircleIcon className="w-5 h-5 mr-3 text-gray-400" />
                <a href="#" className="block px-4 py-2">Support</a>
              </li>
              <li className="flex items-center text-white text-sm hover:bg-gray-700 pl-6">
                <XMarkIcon className="w-5 h-5 mr-3 text-gray-400" />
                <button onClick={handleLogoutClick} className="block px-4 py-2">
                  Logout
                </button>
              </li>
            </ul>
          )}
        </div>
      </div>
      <LogoutModal isOpen={isLogoutModalOpen} onClose={handleModalClose} onError={handleLogoutError} />
    </nav>

  );
}
