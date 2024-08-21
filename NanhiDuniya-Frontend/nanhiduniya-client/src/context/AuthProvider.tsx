import { handleError } from "@/utils/ErrorHandelling/errorHandler";
import { hasTokenExpired } from "@/utils/HasTokenExpred";
import axios from "axios";
import router, { useRouter } from 'next/navigation';
import { createContext, useContext, useEffect, useState } from "react";
import { FieldValues } from 'react-hook-form';
import { toast } from "react-toastify";
type AuthContextValue = {
  isAuthenticated: boolean;
  login: (email: string, password: string) => Promise<void>;
  logout: () => void;
  spinner: boolean;
  setSpinner: React.Dispatch<React.SetStateAction<boolean>>;
};
const AuthContext = createContext<AuthContextValue | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const router = useRouter();
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [spinner, setSpinner] = useState(false);

  const login = async (email: string, password: string) => {
    try {
      await axios.post('/api/users/login', { email, password });
      setIsAuthenticated(true);
      toast.success('Login successful');
      router.push("/admin/dashboard"); // Redirect after login
    } catch (error:any) {
     handleError(error);
    }
  };

  const logout = async () => {
    try {
      await axios.post('/api/auth/logout');
      setIsAuthenticated(false);
      router.push('/auth/login'); // Redirect after logout
    } catch (error) {
      console.error('Logout failed:', error);
      // Handle logout error (e.g., show a toast notification)
    }
  };

  const contextValue: AuthContextValue = {
    isAuthenticated,
    login,
    logout,
    spinner,
    setSpinner
  };

  return (
    <AuthContext.Provider value={contextValue}>
        {children}
    </AuthContext.Provider>
  );
};



export const useAuth = (): AuthContextValue => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};






// import React, { createContext, useContext, useState, useEffect } from 'react';
// import axios from 'axios';

// interface AuthContextValue {
//   isAuthenticated: boolean;
//   login: (email: string, password: string) => Promise<void>;
//   logout: () => void;
// }

// const AuthContext = createContext<AuthContextValue | undefined>(undefined);

// export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
//   const [isAuthenticated, setIsAuthenticated] = useState(false);

//   useEffect(() => {
//     // Optionally, check if a valid session exists on mount
//   }, []);

//   const login = async (email: string, password: string) => {
//     try {
//       await axios.post('/api/auth/login', { email, password });
//       setIsAuthenticated(true);
//     } catch (error) {
//       console.error('Login failed:', error);
//       setIsAuthenticated(false);
//     }
//   };

//   const logout = async () => {
//     await axios.post('/api/auth/logout');
//     setIsAuthenticated(false);
//   };

//   return (
//     <AuthContext.Provider value={{ isAuthenticated, login, logout }}>
//       {children}
//     </AuthContext.Provider>
//   );
// };

// export const useAuth = () => {
//   const context = useContext(AuthContext);
//   if (!context) {
//     throw new Error('useAuth must be used within an AuthProvider');
//   }
//   return context;
// };





