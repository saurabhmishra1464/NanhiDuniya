import { ReactNode } from "react";
import { ModalProvider } from "./ModalProvider";
import { AuthProvider } from "./AuthProvider";
import { SessionProvider } from "next-auth/react";

export default function AppProviders({ children }: { children: ReactNode }) {
  return (
        <ModalProvider>
          {children}
        </ModalProvider>
  );
};