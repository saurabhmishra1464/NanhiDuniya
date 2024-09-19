using Azure;
using Microsoft.AspNetCore.Http;
using NanhiDuniya.Core.Models.Exceptions;
using NanhiDuniya.UserManagement.Api.Extentions;
using NanhiDuniya.UserManagement.Api.Middleware;
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
            var errorMessage = "Something Went Wrong. Please try again later.";
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
                case ArgumentNullException _:
                case ArgumentException _:
                    statusCode = HttpStatusCode.BadRequest;
                    errorMessage = "Invalid request.";
                    break;
                case KeyNotFoundException _:
                    statusCode = HttpStatusCode.NotFound;
                    errorMessage = "Resource not found.";
                    break;
                case InvalidOperationException _:
                    statusCode = HttpStatusCode.Conflict;
                    errorMessage = "Operation cannot be completed due to a conflict in the current state.";
                    break;
                case FailedToRevokeRefreshToken failedToRevokeEx:
                    statusCode = HttpStatusCode.BadRequest;  // Or another appropriate status code
                    errorMessage = failedToRevokeEx.Message;
                    break;
                case FailedToUpdate failedToUpdate:
                    statusCode = HttpStatusCode.BadRequest;  // Or another appropriate status code
                    errorMessage = failedToUpdate.Message;
                    break; 
                case RegsitrationFailed regsitrationFailed:
                    statusCode = HttpStatusCode.BadRequest;  // Or another appropriate status code
                    errorMessage = regsitrationFailed.Message;
                    break;
                case UserAlreadyExistsException _:
                    statusCode = HttpStatusCode.Conflict;
                    errorMessage = "User already exists.";
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    break;
            }
            context.Response.StatusCode = (int)statusCode;
            var response = new ApiResponse<object>(false, errorMessage, null, context.Response.StatusCode);
            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    
    }
}
