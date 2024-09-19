using Microsoft.AspNetCore.Http;
using NanhiDuniya.Services.EmailApi.Extentions;
using Newtonsoft.Json;
using System;
using System.Net;

namespace NanhiDuniya.Services.EmailApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger)
        {
            this._next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }

        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            var errorMessage = "An unexpected error occurred. Please try again later.";
            switch (ex)
            {
                case NotImplementedException _:
                    statusCode = HttpStatusCode.NotImplemented;
                    errorMessage = "This feature is not implemented.";
                    break;
                case UnauthorizedAccessException _:
                    statusCode = HttpStatusCode.Unauthorized;
                    errorMessage = "Unauthorized access.";
                    break;
                case ArgumentException _:
                    statusCode = HttpStatusCode.BadRequest;
                    errorMessage = "Invalid request.";
                    break;
                case KeyNotFoundException _:
                    statusCode = HttpStatusCode.NotFound;
                    errorMessage = "Resource not found.";
                    break;
                case FileNotFoundException _:
                    statusCode = HttpStatusCode.NotFound;
                    errorMessage = "File is Not Found";
                    break;
                
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            var response = new ApiResponse<object>(false, errorMessage, null, context.Response.StatusCode,null);
            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }

}
