"use client";
import "jsvectormap/dist/jsvectormap.css";
import "flatpickr/dist/flatpickr.min.css";
import "@/css/satoshi.css";
import "@/css/style.css";
import React from "react";
import { ToastContainer, toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import AppProviders from "@/context/AppProviders";
import { Provider } from "react-redux";
import StoreProvider from "./StoreProvider";
import { store } from "@/store/store";
export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  
  return (
    <html lang="en">
      <body suppressHydrationWarning={true}>
      <Provider store={store}>
        <AppProviders>
          <div className="dark:bg-boxdark-2 dark:text-bodydark">
            {children}
          </div>
          <ToastContainer />
        </AppProviders>
        </Provider>
      </body>
    </html>
  );
}
