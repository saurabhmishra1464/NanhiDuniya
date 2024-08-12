import axios from 'axios';
import https from 'https';
import { getSession } from 'next-auth/react';

// Create an HTTPS agent that ignores SSL certificate validation
const agent = new https.Agent({
  rejectUnauthorized: false
});

// Create an Axios instance with default settings
const axiosInstance = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
  // withCredentials: true, // Include cookies in requests
  httpsAgent: agent,
  headers: {
    "Content-Type": "application/json; charset=utf-8",
    Accept: "application/json",
    "ngrok-skip-browser-warning": true,
    "Access-Control-Allow-Headers": "*",
  },
});


// Axios request interceptor
axiosInstance.interceptors.request.use(
  async (config) => {
    debugger
    // Retrieve user information from local storage
    const session = await getSession();
    const token = session?.user.token;

    // Add Authorization header if the user has an access token
    if (token) {
      config.headers["Authorization"] = `Bearer ${token}`;
    } else {
      // Set an empty Authorization header if no access token is available
      delete config.headers["Authorization"];
    }
    // You can also modify other parts of the config as needed

    return config;
  },
  (error) => {
    // Do something with request error
    return Promise.reject(error);
  }
);




// axiosInstance.interceptors.response.use(
//   (response) => {
//     // Add logic to handle responses (e.g., logging, global success handling)
//     return response;
//   },
//   (error) => {
//     // Handle response error
//     if (error.response && error.response.status === 401) {
//       // Handle unauthorized access (e.g., redirect to login)
//       window.location.href = '/login';
//     }
//     return Promise.reject(error);
//   }
// );

export default axiosInstance;

export { axiosInstance };
