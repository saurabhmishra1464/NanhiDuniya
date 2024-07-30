// import ResetPassword from '@/model/User';
// import {axiosInstanceUserManagement} from '../../utils/AxiosInstances/api';
// import { ApiResponse } from '../Responses/ApiResponse';
// import axios,{ AxiosError } from 'axios';

// const resetPassword = async (resetPasswordData: ResetPassword) =>{
//     try{
//         
//         const response  = await axiosInstanceUserManagement.post<ApiResponse>('/api/Account/ResetPassword', resetPasswordData);
//         return response.data;
//     }catch (error) {
//         if (axios.isAxiosError(error)) {
//           throw error; // Let the component handle the error
//         }
//         return { statusCode: 500, message: 'Unexpected error occurred.' };
//       }
//     };

// const UserService = {
//     resetPassword,
//   };
//   export default UserService;


import { LoginDto, ResetPassword } from '@/model/User';
import { axiosInstance } from '../../utils/AxiosInstances/api';
import { ApiResponse } from '../Responses/ApiResponse';
import axios, { AxiosError } from 'axios';

const resetPassword = async (resetPasswordData: ResetPassword): Promise<ApiResponse> => {
  try {
    const response = await axiosInstance.post<ApiResponse>('/api/Account/ResetPassword', resetPasswordData);
    return response.data;
  } catch (error) {
    if (axios.isAxiosError(error)) {

      // If error.response is available, it means the error is from the API
      if (error.response) {
        // Pass the error response directly, or you can customize it further
        throw new Error(error.response.data.message || 'An error occurred while resetting the password.');
      } else {
        // If no response was received
        throw new Error('Network error. Please check your connection and try again.');
      }
    } else {
      // For non-Axios errors
      throw new Error('Unexpected error occurred.');
    }
  }
};

const login = async (loginData: LoginDto) => {
  debugger
  try {
    const response = await axiosInstance.post('/api/Account/Login', loginData);
    return response.data;
  } catch (error) {
  }
}

const UserService = {
  resetPassword,
  login
};

export default UserService;
