import { LoginFormValidation, PersonalInfoValidation } from "@/lib/validation";
import { axiosPrivate } from "@/utils/AxiosInstances/api";
import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { z } from "zod";

const initialState: AuthState = {
    user: null,
};

interface AuthState {
    user: User | null;
}

interface User {
    id: string;
    fullName: string;
    userName: string;
    phoneNumber: string;
    email: string;
    bio: string;
    profilePictureUrl: string;
    roles: [];
}

interface AuthResponse {
    success: boolean;
    user: User;
    message: string;
}

interface uploadImageResponse {
    success: boolean;
    message: string;
    profilePictureUrl: string;
}

interface RevokeRefreshTokenRequest {
    userId: string;
}


// export const loginUser = createAsyncThunk<AuthResponse, z.infer<typeof LoginFormValidation>>(
//     "/Account/Login",
//     async (loginForm) => {
//         debugger
//         const response = await axiosPrivate.post<AuthResponse>("api/Account/Login", loginForm);
//         return response.data;
//     });

// export const logoutUser = createAsyncThunk<AuthResponse, RevokeRefreshTokenRequest>(
//     "/Account/RevokeRefreshToken",
//     async (userId) => {
//         const response = await axiosPrivate.post<AuthResponse>("api/Account/RevokeRefreshToken", userId);
//         return response.data;
//     })

// export const checkAuth = createAsyncThunk(
//     "/auth/checkauth",

//     async () => {
//         const response = await axiosPrivate.get("api/Account/check-auth",
//         );
//         return response.data;
//     }
// );

// export const updateUser = createAsyncThunk<AuthResponse, z.infer<typeof PersonalInfoValidation>>(
//     "/Account/UpdateUser",
//     async (payload) => {
//         const response = await axiosPrivate.put<AuthResponse>("api/Account/UpdateUser", payload);
//         return response?.data;
//     }
// );

// export const uploadImage = createAsyncThunk<AuthResponse, FormData>(
//     "/Account/UploadProfilePicture",
//     async (formData) => {
//         debugger
//         const response = await axiosPrivate.post<AuthResponse>("api/Account/UploadProfilePicture", formData,
//             {
//                 headers: {
//                     'Content-Type': 'multipart/form-data',
//                 },
//                 withCredentials: true
//             }
//         );
//         return response.data;
//     });

const authSlice = createSlice({
    name: "auth",
    initialState,
    reducers: {
        setUser: (state, action: PayloadAction<User | null>) => { state.user = action.payload;},
        logOut: (state) => {
                state.user = null;
        }
    },
    // extraReducers: (builder) => {
    //     builder.
    //         addCase(loginUser.pending, (state) => {
    //             state.isLoading = true;
    //             state.isAuthenticated = false;
    //             state.user = null;
    //         })
    //         .addCase(loginUser.fulfilled, (state, action) => {
    //             console.log(action);
    //             state.isLoading = false;
    //             state.user = action.payload.success ? action.payload.user : null;
    //             state.isAuthenticated = action.payload.success;
    //         })
    //         .addCase(loginUser.rejected, (state, action) => {
    //             state.isLoading = false;
    //             state.user = null;
    //             state.isAuthenticated = false;
    //         })
    //         .addCase(checkAuth.pending, (state) => {
    //             state.isLoading = true;
    //         })
    //         .addCase(checkAuth.fulfilled, (state, action) => {
    //             state.isLoading = false;
    //             state.user = action.payload.success ? action.payload.user : null;
    //             state.isAuthenticated = action.payload.success;
    //         })
    //         .addCase(checkAuth.rejected, (state, action) => {
    //             state.isLoading = false;
    //             state.user = null;
    //             state.isAuthenticated = false;
    //         })
    //         .addCase(logoutUser.fulfilled, (state, action) => {
    //             state.isLoading = false;
    //             state.user = null;
    //             state.isAuthenticated = false;
    //         })
    //         .addCase(updateUser.pending, (state) => {
    //             state.isLoading = true;
    //         })
    //         .addCase(updateUser.fulfilled, (state, action) => {
    //             state.isLoading = false;
    //             state.user = action.payload.user;
    //         })
    //         .addCase(updateUser.rejected, (state) => {
    //             state.isLoading = false;
    //         })
    //         .addCase(uploadImage.pending, (state) => {
    //             state.isLoading = true;
    //         })
    //         .addCase(uploadImage.fulfilled, (state, action) => {
    //             state.isLoading = false;
    //             if (state.user && action.payload.user.profilePictureUrl) {
    //                 state.user.profilePictureUrl = action.payload.user.profilePictureUrl;
    //             }
    //         })
    //         .addCase(uploadImage.rejected, (state) => {
    //             state.isLoading = false;
    //         })
    // },
});

export const { setUser,logOut } = authSlice.actions;
export default authSlice.reducer;
