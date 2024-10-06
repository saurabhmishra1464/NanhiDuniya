// export interface getUserProfileResponse {
//     success: boolean;
//     userProfile: UserProfile;
//     message: string;
// }

export interface UserProfile {
    id: string;
    fullName: string;
    userName: string;
    phoneNumber: string;
    email: string;
    bio: string;
    profilePictureUrl: string;
    roles: [];
    iEmailConfirmed: boolean;
}

export interface UserProfile {
    id: string;
    fullName: string;
    userName: string;
    phoneNumber: string;
    email: string;
    bio: string;
    profilePictureUrl: string;
    roles: [];
    iEmailConfirmed: boolean;
}

export interface LogoutResponse {
    success: boolean;
    message: string;
}

export interface RevokeRefreshTokenRequest{
    userId: string;
}

export interface uploadImageResponse {
    profilePictureUrl: string;
}