export default interface ResetPassword {
    email: string;
    token: string;
    newPassword: string;
}