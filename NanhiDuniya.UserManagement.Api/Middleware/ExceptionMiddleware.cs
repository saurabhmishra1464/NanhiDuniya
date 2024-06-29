using NanhiDuniya.Core.Models.ErrorHandle;
using NanhiDuniya.Core.Models.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace NanhiDuniya.UserManagement.APi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            var errorDetails = new ErrorDeatils
            {
                ErrorType = "Failure",
                ErrorMessage = ex.Message,
            };

            switch (ex)
            {
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    errorDetails.ErrorType = "Not Found";
                    break;
                case BadRequestException badRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorDetails.ErrorType = "Bad Request";
                    break;
                default:
                    break;
            }

            string response = JsonConvert.SerializeObject(errorDetails);
            context.Response.StatusCode = (int)statusCode;
            return context.Response.WriteAsync(response);
        }
    }

    public class ErrorDeatils
    {
        public string ErrorType { get; set; }
        public string ErrorMessage { get; set; }
    }
}


















//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Net;
//using System.Threading.Tasks;
//using Newtonsoft.Json;

//public class ExceptionMiddleware
//{
//    private readonly RequestDelegate _next;
//    private readonly ILogger<ExceptionMiddleware> _logger;

//    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
//    {
//        _next = next;
//        _logger = logger;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        try
//        {
//            await _next(context);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "An unhandled exception has occurred.");
//            await HandleExceptionAsync(context, ex);
//        }
//    }

//    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
//    {
//        context.Response.ContentType = "application/json";

//        var response = new
//        {
//            ErrorType = "Failure",
//            ErrorMessage = "An unexpected error occurred. Please try again later."
//        };

//        // Log detailed exception information
//        if (context.Request.IsLocal()) // Check if the request is local
//        {
//            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//            var detailedError = new
//            {
//                ErrorType = "Failure",
//                ErrorMessage = exception.Message,
//                StackTrace = exception.StackTrace,
//                InnerException = exception.InnerException?.Message
//            };

//            _logger.LogError(exception, "Detailed error information:");
//            return context.Response.WriteAsync(JsonConvert.SerializeObject(detailedError));
//        }
//        else
//        {
//            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
//        }
//    }
//}
