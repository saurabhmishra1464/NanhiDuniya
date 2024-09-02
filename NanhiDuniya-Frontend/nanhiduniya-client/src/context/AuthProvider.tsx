'use client'
import LoadingSpinner from "@/components/LoadingSpinner";
import useUser from "@/hooks/useUsers";
import { axiosPrivate, axiosPublic } from "@/utils/AxiosInstances/api";
import { handleError } from "@/utils/ErrorHandelling/errorHandler";
import axios, { HttpStatusCode, InternalAxiosRequestConfig } from "axios";
import { useRouter } from 'next/navigation';
import { createContext, useContext, useEffect, useLayoutEffect, useState } from "react";
import { toast } from "react-toastify";
type AuthContextValue = {
  login: (email: string, password: string) => Promise<void>;
  // logout: () => void;
  loading: boolean;
  setLoading: React.Dispatch<React.SetStateAction<boolean>>;
  loginError: string;
};
const AuthContext = createContext<AuthContextValue | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const router = useRouter();
  const [loading, setLoading] = useState(false);
  const [loginError, setLoginError] = useState("");

  const login = async (email: string, password: string) => {
    setLoading(true);
    try {
      const response = await axiosPublic.post('/api/Account/Login', { email, password });
      if (response.status === HttpStatusCode.Ok) {
        console.log(response.data);
        toast.success("You are now signed in!");
        router.push("/admin/dashboard");
        setLoading(false);
      }
    } catch (error) {
      setLoading(false);
      const loginError = handleError(error as Error);
      toast.error(loginError);
      setLoginError(loginError);
    } finally {
      setLoading(false);
    }
  };

  // const logout = async () => {
  //   try {
  //     await axios.post('/api/auth/logout');
  //     setIsAuthenticated(false);
  //     router.push('/auth/login'); // Redirect after logout
  //   } catch (error) {
  //     console.error('Logout failed:', error);
  //     // Handle logout error (e.g., show a toast notification)
  //   }
  // };

//   const refresh = async () => {
//   await axiosPublic.get('/api/Account/refresh');
// }

//     useLayoutEffect(() => {
//     debugger
//     const responseIntercept = axiosPrivate.interceptors.response.use(
//       response => response,
//       async (error) => {
//           const prevRequest = error?.config;
//           if (error?.response?.status === 401 && !prevRequest?.sent) {
//               prevRequest.sent = true;
//               const newAccessToken = await refresh();
//               return axiosPrivate(prevRequest);
//           }
//           return Promise.reject(error);
//       }
//   );

//   return () => {
//     axiosPrivate.interceptors.response.eject(responseIntercept);
// };
// }, [])


  const contextValue: AuthContextValue = {
    login,
    // logout,
    loading,
    setLoading,
    loginError,
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





