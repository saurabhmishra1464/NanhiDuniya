import axios from "axios";

export function handleError(error: Error) {
    let errorMessage = 'An unexpected error occurred';
    if (axios.isAxiosError(error)) {
        switch (error.response?.status) {
            case 400:
                errorMessage = 'Bad Request: Please check your input.';
                break;
            case 401:
                errorMessage = 'Unauthorized: Please check your credentials.';
                break;
            case 403:
                errorMessage = 'Forbidden: You do not have permission to access this resource.';
                break;
            case 404:
                errorMessage = 'Not Found: The requested resource was not found.';
                break;
            case 500:
                errorMessage = 'Internal Server Error: An error occurred on the server.';
                break;
            default:
                errorMessage = 'An unknown error occurred';
                break;
        }
    } else {
        errorMessage = error.message || 'A network error occurred';
    }
    return errorMessage;
}