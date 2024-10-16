import { AdminCreateValidation, LoginFormValidation, PersonalInfoValidation } from '@/lib/validation'
import { ApiResponse } from '@/model/Responses/ApiResponse'
import { Admins, LogoutResponse, RevokeRefreshTokenRequest, uploadImageResponse, UserProfile } from '@/model/Responses/AuthResponse'
import { baseQueryWithReauth } from '@/utils/fetchBaseQuery'
import { 
  createApi, 
} from '@reduxjs/toolkit/query/react'
import { z } from 'zod'



export const authApi = createApi({
  baseQuery: baseQueryWithReauth,
  tagTypes: ["UserManagement"],
  endpoints: (builder) => ({

    registerAdmin: builder.mutation<ApiResponse<object>, z.infer<typeof AdminCreateValidation>>({
      query:(user) =>({
        url: 'api/Account/Register-Admin',
        method: "POST",
        body:user,
      }),
      invalidatesTags: ["UserManagement"],
    }),

    loginUser: builder.mutation<ApiResponse<UserProfile>,z.infer<typeof LoginFormValidation>>({
      query: (user) => ({
        url: '/api/Account/Login',
        method: 'POST',
        body:user,
      }),
    }),

    getUserProfile: builder.query<ApiResponse<UserProfile>, void>({
      query: () => '/api/Account/me',
      providesTags: ["UserManagement"],
    }),

    getAdmins: builder.query<ApiResponse<Admins>, void>({
      query: () => '/api/Account/AdminList',
      providesTags: ["UserManagement"],
    }),

    updateUser: builder.mutation<ApiResponse<UserProfile>, z.infer<typeof PersonalInfoValidation>>({
      query:(user) =>({
        url: 'api/Account/UpdateUser',
        method: "PUT",
        body:user,
      }),
      invalidatesTags: ["UserManagement"],
    }),

    uploadImage: builder.mutation<ApiResponse<uploadImageResponse>, FormData>({
      query:(userImage) =>({
        url: 'api/Account/UploadProfilePicture',
        method: "POST",
        body:userImage,
      }),
      invalidatesTags: ["UserManagement"],
    }),

    logoutUser: builder.mutation<LogoutResponse,RevokeRefreshTokenRequest>({
      query: (body) => ({
        url: '/api/Account/RevokeRefreshToken',
        method: 'POST',
        body,
      }),
    })
  }),
});


export const {useGetAdminsQuery,useRegisterAdminMutation,useLoginUserMutation, useGetUserProfileQuery, useUpdateUserMutation, useUploadImageMutation, useLogoutUserMutation } = authApi