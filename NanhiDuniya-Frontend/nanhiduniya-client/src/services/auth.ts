import { LoginFormValidation, PersonalInfoValidation } from '@/lib/validation'
import { getUserProfileResponse, LogoutResponse, RevokeRefreshTokenRequest, uploadImageResponse } from '@/model/Responses/AuthResponse'
import { baseQueryWithReauth } from '@/utils/fetchBaseQuery'
import { 
  createApi, 
} from '@reduxjs/toolkit/query/react'
import { z } from 'zod'



export const authApi = createApi({
  baseQuery: baseQueryWithReauth,
  tagTypes: ["UserManagement"],
  endpoints: (builder) => ({

    loginUser: builder.mutation<getUserProfileResponse,z.infer<typeof LoginFormValidation>>({
      query: (user) => ({
        url: '/api/Account/Login',
        method: 'POST',
        body:user,
      }),
    }),

    getUserProfile: builder.query<getUserProfileResponse, void>({
      query: () => '/api/Account/me',
      providesTags: ["UserManagement"],
    }),

    updateUser: builder.mutation<getUserProfileResponse, z.infer<typeof PersonalInfoValidation>>({
      query:(user) =>({
        url: 'api/Account/UpdateUser',
        method: "PUT",
        body:user,
      }),
      invalidatesTags: ["UserManagement"],
    }),

    uploadImage: builder.mutation<uploadImageResponse, FormData>({
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


export const {useLoginUserMutation, useGetUserProfileQuery, useUpdateUserMutation, useUploadImageMutation, useLogoutUserMutation } = authApi