import axios from 'axios';
import https from 'https';
import { getSession } from 'next-auth/react';
import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';
// Create an HTTPS agent that ignores SSL certificate validation
const agent = new https.Agent({
  rejectUnauthorized: false
});

// Create an Axios instance with default settings
const axiosInstance = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
   withCredentials: true, // Include cookies in requests
  httpsAgent: agent,
  headers: {
    Accept: "application/json",
    "ngrok-skip-browser-warning": true,
    "Access-Control-Allow-Headers": "*",
  },
});


// Axios request interceptor
// axiosInstance.interceptors.request.use(
//   async (config) => {
//     // Retrieve user information from local storage
//     const session = await getSession();
//     const token = session?.user.token;

//     // Add Authorization header if the user has an access token
//     if (token) {
//       config.headers["Authorization"] = `Bearer ${token}`;
//     } else {
//       // Set an empty Authorization header if no access token is available
//       delete config.headers["Authorization"];
//     }
//     // You can also modify other parts of the config as needed

//     return config;
//   },
//   (error) => {
//     // Do something with request error
//     return Promise.reject(error);
//   }
// );


// axiosInstance.interceptors.response.use(
//   (response) => {
//     return response;
//   },
//   (error) => {
//     if (error.response && error.response.status === 401) {
//       // Handle unauthorized access (e.g., redirect to login)
//       window.location.href = '/auth/login';
//     }
//     return Promise.reject(error);
//   }
// );

export default axiosInstance;

// export { axiosInstance };
