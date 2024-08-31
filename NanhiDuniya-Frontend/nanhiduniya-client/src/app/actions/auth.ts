"use server"
import { LoginFormValidation } from "@/lib/validation"
import { redirect } from 'next/navigation';
import { z } from "zod";
import { handleError } from "@/utils/ErrorHandelling/errorHandler";
import  axiosInstance  from "@/utils/AxiosInstances/api";
import axios from "axios";

type loginUser = z.infer<typeof LoginFormValidation>

export async function login(data: loginUser) {
  try {
    const response = await axios.post('http://localhost:5001/api/Account/Login', {
      email: data.userName,
      password: data.password,
    },{withCredentials: true});

    if (response?.status === 200) {
      console.log(response);
      return { success: true, message: "Login Successful" };
    } else {
      return { success: false, errors: response.data };
    }
  } catch (error) {
    console.log(error);
    const message = handleError(error as Error);
    return { success: false, errors: message };
  }
}

