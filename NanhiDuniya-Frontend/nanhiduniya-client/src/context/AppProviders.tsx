import { ReactNode, useEffect } from "react";
import { ModalProvider } from "./ModalProvider";
import { AuthProvider } from "./AuthProvider";
import { SessionProvider } from "next-auth/react";
import { useDispatch, useSelector } from "react-redux";
import { AppDispatch, RootState } from "@/store/store";
import Skeleton from "react-loading-skeleton";

export default function AppProviders({ children }: { children: ReactNode }) {

  return (
        <ModalProvider>
          {children}
        </ModalProvider>
  );
};