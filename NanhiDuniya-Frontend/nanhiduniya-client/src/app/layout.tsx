"use client";
import "jsvectormap/dist/jsvectormap.css";
import "flatpickr/dist/flatpickr.min.css";
import "@/css/satoshi.css";
import "@/css/style.css";
import React, {useState } from "react";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import AppProviders from "@/context/AppProviders";

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  
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
