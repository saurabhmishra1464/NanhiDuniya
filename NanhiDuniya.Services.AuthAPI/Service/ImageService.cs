using Microsoft.AspNetCore.Identity;
using NanhiDuniya.Services.AuthAPI.Models;
using NanhiDuniya.Services.AuthAPI.Models.Dto;
using NanhiDuniya.Services.AuthAPI.Service.IService;
using NanhiDuniya.Services.AuthAPI.Utilities;

namespace NanhiDuniya.Services.AuthAPI.Service
{
    public class ImageService : IImageService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly string _storagePath;
        public ImageService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ApiResponse<UploadImageResponse>> SaveImageAsync(UploadProfilePictureDto upload)
        {
            if (upload.formFile == null || upload.formFile.Length == 0)
            {
                return ApiResponseHelper.CreateErrorResponse<UploadImageResponse>("File is empty,Please attach Image.", StatusCodes.Status400BadRequest);
            }
            UploadImageResponse resultResponse = new();
            var user = await _userManager.FindByIdAsync(upload.Id);
            if (user == null)
            {
                return ApiResponseHelper.CreateErrorResponse<UploadImageResponse>("User not found.", StatusCodes.Status404NotFound);
            }

            using (var memoryStream = new MemoryStream())
            {
                await upload.formFile.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();
                var base64String = Convert.ToBase64String(imageBytes);
                var profilePictureUrl = $"data:{upload.formFile.ContentType};base64,{base64String}";
                user.ProfilePictureUrl = profilePictureUrl;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded) { return ApiResponseHelper.CreateErrorResponse<UploadImageResponse>("Failed to update Image. Please try again or contact support if the problem persists.", StatusCodes.Status400BadRequest); }
                resultResponse.ProfilePictureUrl = profilePictureUrl;
                return ApiResponseHelper.CreateSuccessResponse<UploadImageResponse>(resultResponse, "Image Uploaded Successfully");
            }
        }
    }
}
