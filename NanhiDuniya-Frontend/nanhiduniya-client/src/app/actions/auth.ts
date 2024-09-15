"use server"
import { LoginFormValidation } from "@/lib/validation"
import { redirect } from 'next/navigation';
import { z } from "zod";
import { handleError } from "@/utils/ErrorHandelling/errorHandler";
import axios from "axios";
import { axiosPrivate } from "@/utils/AxiosInstances/api";
import { parseStringify } from "@/lib/utils";

type loginUser = z.infer<typeof LoginFormValidation>




export const logoutAccount = async (userId: string) => {
  try {
  await axiosPrivate.post('/api/Account/RevokeRefreshToken', { userId: userId});
  } catch (error) {
    return null;
  }
}


