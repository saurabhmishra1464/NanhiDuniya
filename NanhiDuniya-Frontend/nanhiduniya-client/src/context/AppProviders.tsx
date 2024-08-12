import { ReactNode } from "react";
import AuthProvider from "./AuthProvider";
import { ModalProvider } from "./ModalProvider";

export default function AppProviders({ children }: { children: ReactNode }) {
  return (
    <AuthProvider>
        <ModalProvider>
          {children}
        </ModalProvider>
    </AuthProvider>
  );
};