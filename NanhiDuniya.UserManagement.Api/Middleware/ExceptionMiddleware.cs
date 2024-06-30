﻿using Microsoft.AspNetCore.Http;
using NanhiDuniya.Core.Models.Exceptions;
using NanhiDuniya.UserManagement.Api.Extentions;
using Newtonsoft.Json;
using System;
using System.Net;

namespace NanhiDuniya.UserManagement.APi.Middleware
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
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }

            var apiResponse = new ApiResponse((int)statusCode, errorMessage);
            context.Response.StatusCode = (int)statusCode;
            string response = JsonConvert.SerializeObject(apiResponse);
            return context.Response.WriteAsync(response);
        }
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
