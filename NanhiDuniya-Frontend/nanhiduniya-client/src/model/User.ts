export interface ResetPassword {
    email: string;
    token: string;
    password: string;
    newPassword: string;
}

export interface LoginDto {
    email: string;
    password: string;
}
