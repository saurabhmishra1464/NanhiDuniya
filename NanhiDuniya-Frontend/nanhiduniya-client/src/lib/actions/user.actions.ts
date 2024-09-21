'use server';

import { axiosPrivate, axiosPublic } from "@/utils/AxiosInstances/api";
import { handleError } from "@/utils/ErrorHandelling/errorHandler";

export async function logoutAccount(userId: string) {
  //   try {
  //     const response = await axiosPublic.post('/api/Account/RevokeRefreshToken', {userId});
  // console.log(response);
  //     if (response?.status === 200) {
  //       console.log(response);
  //       return { success: true, message: "Logout Successful" };
  //     } else {
  //       return { success: false, errors: response.data };
  //     }
  //   } catch (error) {
  //     console.log(error);
  //     const message = handleError(error as Error);
  //     return { success: false, errors: message };
  //   }
  }