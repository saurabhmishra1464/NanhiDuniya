"use client";
import React, { useEffect, useState } from "react";
import { useModal } from "@/context/ModalProvider";
import { ModalTypes } from "@/enums/modalTypes";
import Sidebar from "@/components/Sidebar";
import Header from "@/components/Header";
import LogoutModal from "@/components/modals/LogoutModal";
import { useAuth } from "@/context/AuthProvider";
import { Provider, useDispatch } from "react-redux";
import { logOut, setUser } from "@/store/auth-slice";
import { useGetUserProfileQuery } from "@/services/auth";
// Lazy load components
// const Sidebar = lazy(() => import('@/components/Sidebar'));
// const Header = lazy(() => import('@/components/Header'));
// const LogoutModal = lazy(() => import('@/components/modals/LogoutModal'));

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const { activeModal, closeModal,logout } = useModal();
  const { data: user, isLoading, isError } = useGetUserProfileQuery();
  const dispatch = useDispatch()

  useEffect(() => {
    if (!isLoading) {
      if (user) {
        dispatch(setUser(user.userProfile));  // Set user in store
      } else {
        dispatch(logOut());   // Clear user from store
      }
    }
  }, [user, isLoading, isError, dispatch]);
  
  // const Fallback = () => (
  //   <div className="flex justify-center items-center h-screen">
  //     <div className="animate-spin h-8 w-8 border-t-2 border-blue-500 rounded-full"></div>
  //     <span className="ml-4 text-gray-500">Component Loading...</span>
  //   </div>
  // );
  return (
      <div className="flex">
        <Sidebar sidebarOpen={sidebarOpen} setSidebarOpen={setSidebarOpen} />
        <div className="relative flex flex-1 flex-col lg:ml-72.5">
          <Header sidebarOpen={sidebarOpen} setSidebarOpen={setSidebarOpen} />

          <main>
            <div className="mx-auto max-w-screen-2xl p-4 md:p-6 2xl:p-10">
          
              {children}
              
              {activeModal === ModalTypes.LOGOUT && <LogoutModal onClose={closeModal} logout = {logout} />}
            </div>
          </main>
        </div>
      </div>
  );
}
