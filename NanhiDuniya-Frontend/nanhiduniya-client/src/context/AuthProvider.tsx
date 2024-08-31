// // import axiosInstance from "@/utils/AxiosInstances/api";
// import { axiosInstance } from "@/utils/AxiosInstances/api";
// import axios, { InternalAxiosRequestConfig } from "axios";
// import { useRouter } from 'next/navigation';
// import { createContext, useContext, useEffect, useLayoutEffect, useState } from "react";
// type AuthContextValue = {
//   isAuthenticated: boolean;
//   login: (email: string, password: string) => Promise<void>;
//   // logout: () => void;
//   spinner: boolean;
//   setSpinner: React.Dispatch<React.SetStateAction<boolean>>;
// };
// const AuthContext = createContext<AuthContextValue | undefined>(undefined);

// export function AuthProvider({ children }: { children: React.ReactNode }) {
//   const router = useRouter();
//   const [isAuthenticated, setIsAuthenticated] = useState(false);
//   const [spinner, setSpinner] = useState(false);
//   const [token, setToken] = useState<string | null>(null);

//   interface CustomAxiosRequestConfig extends InternalAxiosRequestConfig {
//     _retry?: boolean;
//   }

//   useEffect(() => {
//     const fetchMe = async () => {
//       try {
//         console.log("Fetching user data...");
//         const response = await axiosInstance.get('/api/Account/me');
//         console.log("Response from /api/Account/me:", response);
//         if(response.data && response.data.accessToken){
//           setToken(response.data.accessToken);
//         }
//         else{
//           console.log("No accessToken in response");
//           setToken(null);
//         }
//       } catch(error) {
//         console.error("Error fetching user data:", error);
//         setToken(null);
//       }
//     };

//     fetchMe();
//   }, []);


//   useLayoutEffect(() => {
//     const authInterceptor = axiosInstance.interceptors.request.use(
//       (config: CustomAxiosRequestConfig) => {
//         console.log("Test config",config._retry);
//         config.headers.Authorization =
//           !config._retry && token
//             ? `Bearer ${token}`
//             : config.headers.Authorization;
//         return config;
//       }
//     );

//     return () => {
//       axiosInstance.interceptors.request.eject(authInterceptor);
//     };
//   }, [token]);

//   useLayoutEffect(() => {
//     debugger
//     const refreshInterceptor = axiosInstance.interceptors.response.use(
//       async (response) => {
//         return response;
//       },
//       async (error) => {
//         console.log("inside error 123232323",error);
//         const originalRequest = error.config as CustomAxiosRequestConfig;
//        console.log(error.response.status);
//        console.log(error.response.data.message);
//         if (
//          ( error.response.status === 401 || error.response.status === 403)
//           && !originalRequest._retry  && error.config.url !== '/api/Account/RefreshToken'
//         ) {
//           try {
//             const response = await axiosInstance.post('/api/Account/RefreshToken');
//             setToken(response.data.accessToken);
//             originalRequest.headers.Authorization = `Bearer ${response.data.accessToken}`;
//             originalRequest._retry = true;
//             return axiosInstance(originalRequest);
//           } catch {
//             setToken(null);
//           }
//         }

//         return Promise.reject(error);
//       }
//     );

//     return () => {
//       axiosInstance.interceptors.response.eject(refreshInterceptor);
//     };
//   }, [token]);

//   const login = async (email: string, password: string) => {
//     await axios.post('http://localhost:5001/api/Account/Login', { email, password },{withCredentials:true})
//   };

//   // const logout = async () => {
//   //   try {
//   //     await axios.post('/api/auth/logout');
//   //     setIsAuthenticated(false);
//   //     router.push('/auth/login'); // Redirect after logout
//   //   } catch (error) {
//   //     console.error('Logout failed:', error);
//   //     // Handle logout error (e.g., show a toast notification)
//   //   }
//   // };

//   const contextValue: AuthContextValue = {
//     isAuthenticated,
//     login,
//     // logout,
//     spinner,
//     setSpinner
//   };

//   return (
//     <AuthContext.Provider value={contextValue}>
//       {children}
//     </AuthContext.Provider>
//   );
// };



// export const useAuth = (): AuthContextValue => {
//   const context = useContext(AuthContext);
//   if (!context) {
//     throw new Error("useAuth must be used within an AuthProvider");
//   }
//   return context;
// };








import axiosInstance from "@/utils/AxiosInstances/api";
import { handleError } from "@/utils/ErrorHandelling/errorHandler";
import axios, { HttpStatusCode } from "axios";
import { useRouter } from 'next/navigation';
import { createContext, useContext, useEffect, useState } from "react";
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
  
  useEffect(() => {
    const fetchMe = async () => {
      try {
        console.log("Fetching user data...");
        const response = await axiosInstance.get('/api/Account/me');
        console.log("Response from /api/Account/me:", response);
        if(response.data && response.data.accessToken){
         
        }
        else{
          console.log("No accessToken in response");
          
        }
      } catch(error) {
        console.error("Error fetching user data:", error);
      
      }
    };

    fetchMe();
  }, []);


  const login = async (email: string, password: string) => {
    setLoading(true);
    try {
      const response = await axios.post('http://localhost:5001/api/Account/Login', { email, password }, { withCredentials: true });
      if (response.status === HttpStatusCode.Ok) {
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

  const contextValue: AuthContextValue = {
    login,
    // logout,
    loading,
    setLoading,
    loginError
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





