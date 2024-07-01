using System.Net;

namespace NanhiDuniya.Services.EmailApi.Resources
{
    public class ResponseDTO
    {
        public bool OperationSuccess { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public object? Errors { get; set; }
    }
}
