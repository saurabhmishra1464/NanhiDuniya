"use client";
import "jsvectormap/dist/jsvectormap.css";
import "flatpickr/dist/flatpickr.min.css";
import "@/css/satoshi.css";
import "@/css/style.css";
import React, { useEffect, useState } from "react";
import AuthProvider from "@/context/AuthProvider";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css"
import AppProviders from "@/context/AppProviders";

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  const [sidebarOpen, setSidebarOpen] = useState(false);
  const [loading, setLoading] = useState<boolean>(true);

  return (
    <html lang="en">
      <body suppressHydrationWarning={true}>
        <AppProviders>
          <div className="dark:bg-boxdark-2 dark:text-bodydark">
            {children}
          </div>
          <ToastContainer />
        </AppProviders>
      </body>
    </html>
  );
}
