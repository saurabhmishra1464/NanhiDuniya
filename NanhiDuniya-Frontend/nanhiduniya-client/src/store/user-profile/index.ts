// import { PersonalInfoValidation } from "@/lib/validation";
// import { axiosPrivate } from "@/utils/AxiosInstances/api";
// import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
// import { z } from "zod";

// const initialState: Profile = {
//     isLoading: false,
//     userProfile: null,
// };

// interface Profile {
//     isLoading: boolean;
//     userProfile: UserProfile | null;
// }

// interface UserProfile {
//     id: string;
//     fullName: string;
//     userName: string;
//     phoneNumber: string;
//     email: string;
//     bio: string;
//     profilePictureUrl: string;
//     roles: [];
// }

// interface getUserProfileResponse {
//     success: boolean;
//     userProfile: UserProfile;
//     message: string;
// }

// interface uploadImageResponse {
//     success: boolean;
//     message: string;
//     profilePictureUrl: string;
// }

// // export const getUserProfile = createAsyncThunk<getUserProfileResponse>(
// //     "/Account/me",
// //     async () => {
// //         const response = await axiosPrivate.get<getUserProfileResponse>("/api/Account/me");
// //         return response?.data
// //     }
// // );

// // export const updateUser = createAsyncThunk<getUserProfileResponse, z.infer<typeof PersonalInfoValidation>>(
// //     "/Account/UpdateUser",
// //     async (payload) => {
// //         const response = await axiosPrivate.put<getUserProfileResponse>("api/Account/UpdateUser", payload);
// //         return response?.data;
// //     }
// // );

// // export const uploadImage = createAsyncThunk<uploadImageResponse, FormData>(
// //     "/Account/UploadProfilePicture",
// //     async (formData) => {
// //         debugger
// //         const response = await axiosPrivate.post<uploadImageResponse>("api/Account/UploadProfilePicture", formData,
// //             {
// //                 headers: {
// //                     'Content-Type': 'multipart/form-data',
// //                 },
// //                 withCredentials: true
// //             }
// //         );
// //         return response.data;
// //     });

// const UserProfileSlice = createSlice({
//     name: "userProfile",
//     initialState,
//     reducers: {},
//     extraReducers: (builder) => {
//         builder
//             .addCase(getUserProfile.pending, (state) => {
//                 state.isLoading = true;
//             })
//             .addCase(getUserProfile.fulfilled, (state, action) => {
//                 state.isLoading = false;
//                 state.userProfile = action.payload.userProfile;
//             })
//             .addCase(getUserProfile.rejected, (state, action) => {
//                 state.isLoading = false;
//                 state.userProfile = null;
//             })
//             // .addCase(uploadImage.pending, (state) => {
//             //     state.isLoading = true;
//             // })
//             // .addCase(uploadImage.fulfilled, (state, action) => {
//             //     state.isLoading = false;
//             //     if (state.userProfile && action.payload.profilePictureUrl) {
//             //         state.userProfile.profilePictureUrl = action.payload.profilePictureUrl;
//             //     }
//             // })
//             // .addCase(uploadImage.rejected, (state) => {
//             //     state.isLoading = false;
//             // })
//             // .addCase(updateUser.pending, (state) => {
//             //     state.isLoading = true;
//             // })
//             // .addCase(updateUser.fulfilled, (state, action) => {
//             //     state.isLoading = false;
//             //     if (state.userProfile && action.payload.userProfile) {
//             //         state.userProfile = action.payload.userProfile;
//             //     }
//             // })
//             // .addCase(updateUser.rejected, (state) => {
//             //     state.isLoading = false;
//             // });
//     },
// });

// export default UserProfileSlice.reducer;