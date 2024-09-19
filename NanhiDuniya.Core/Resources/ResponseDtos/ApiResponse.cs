using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NanhiDuniya.Core.Resources.ResponseDtos
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
            Errors = errors;
        }
    }

}
