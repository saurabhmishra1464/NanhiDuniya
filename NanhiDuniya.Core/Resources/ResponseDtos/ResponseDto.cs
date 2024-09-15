using NanhiDuniya.Core.Resources.AccountDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Core.Resources.ResponseDtos
{
    public class ResponseDTO
    {
        public bool OperationSuccess { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public object? Errors { get; set; }
    }

    public class ResultResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class LoginResponse: ResultResponse
    {
        public UserProfile User { get; set; }
        //public bool IsEmailConfirmed { get; set; }
    }

    public class RefreshTokenResponse: ResultResponse
    {


    }

    public class LogoutResponse: ResultResponse
    {

    }

    public class GetUserProfileResponse: ResultResponse
    {
        public UserProfile userProfile { get; set; }
    }

    public class UploadImageResponse : ResultResponse
    {
        public string? ProfilePictureUrl { get; set; }
    }

    public class UpdateUserResponse: ResultResponse
    {
      public UserProfile userProfile { get; set; }
    }

    public class UserProfile
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string? UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public List<string>? Roles { get; set; }
    }
}
