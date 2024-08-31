"use client";
import React, { useState } from "react";
import { useModal } from "@/context/ModalProvider";
import { ModalTypes } from "@/enums/modalTypes";
import Sidebar from "@/components/Sidebar";
import Header from "@/components/Header";
import LogoutModal from "@/components/modals/LogoutModal";
import { useAuth } from "@/context/AuthProvider";

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
  const { activeModal, closeModal } = useModal();
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
              {activeModal === ModalTypes.LOGOUT && <LogoutModal onClose={closeModal} />}
            </div>
          </main>
        </div>
      </div>
  );
}
