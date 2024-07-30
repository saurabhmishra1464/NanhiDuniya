import axios from 'axios';
import https from 'https';

// Create an HTTPS agent that ignores SSL certificate validation
const agent = new https.Agent({  
  rejectUnauthorized: false
});

// Create an Axios instance with default settings
const axiosInstance = axios.create({
  baseURL: 'https://localhost:7777',
  httpsAgent: agent,
  headers: {
    'Content-Type': 'application/json',
  },
});

export default axiosInstance;


// // Centralized error handler
// const handleErrorResponse = (error: AxiosError<any>) => {
//     
//   if (error.response) {
//     // The request was made and the server responded with a status code
//     console.error('Response error (User Management API):', error.response);
//     const errorMessage = error.response.data.message || 'An error occurred.';
//     return Promise.reject(new Error(errorMessage));
//   } else if (error.request) {
//     // The request was made but no response was received
//     console.error('No response received (User Management API):', error.request);
//     return Promise.reject(new Error('Network error. Please try again later.'));
//   } else {
//     // Something happened in setting up the request that triggered an error
//     console.error('Error (User Management API):', error.message);
//     return Promise.reject(new Error('Unexpected error occurred.'));
//   }
// };

// Add a response interceptor for axiosInstanceUserManagement
// axiosInstanceUserManagement.interceptors.response.use(
//   response => response,
//   (error: AxiosError<any>) => handleErrorResponse(error)
// );


// Axios instance for another microservice
// const axiosInstanceB = axios.create({
//   baseURL: 'https://microservice-b-url/api',
//   timeout: 5000,
//   headers: {
//     'Content-Type': 'application/json',
//   },
// });

export { axiosInstance};
