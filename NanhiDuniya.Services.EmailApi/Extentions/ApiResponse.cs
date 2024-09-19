namespace NanhiDuniya.Services.EmailApi.Extentions
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int StatusCode { get; set; }
        public object? Errors { get; set; }

        public ApiResponse(bool success, string message, T data, int statusCode, object? errors)
        {
            Success = success;
            Message = message;
            Data = data;
            StatusCode = statusCode;
            StatusCode = statusCode;
            Errors = errors;
        }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            {
                case 200:
                    return "Ok";
                case 201:
                    return "Created";
                case 204:
                    return "No record found";
                case 205:
                    return "No User Found. Email address or password is incorrect. Please try again or contact your coach to see if you have been added to the team.";
                case 206:
                    return "No Default Team Found.";
                case 207:
                    return "The email doesn't exists in database";
                case 208:
                    return "Operation failure!";
                case 302:
                    return "Already exists!";
                case 400:
                    return "Bad request!";
                case 401:
                    return "Unauthorized";
                case 404:
                    return "Resource not found!";
                case 500:
                    return "Internal Server Error.";
                default:
                    return null;
            }
        }
    }
}
