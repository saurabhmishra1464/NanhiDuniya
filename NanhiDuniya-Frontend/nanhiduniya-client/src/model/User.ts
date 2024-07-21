export interface ResetPassword {
    email: string;
    token: string;
    newPassword: string;
}

export interface LoginDto {
    email: string;
    password: string;
}
