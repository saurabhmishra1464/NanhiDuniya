using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NanhiDuniya.Core.Interfaces;
using NanhiDuniya.Core.Models;
using NanhiDuniya.Core.Resources.AccountDtos;
using NanhiDuniya.Core.Resources.ResponseDtos;
using NanhiDuniya.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Service.Services
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
                return new ApiResponse<UploadImageResponse>(false, "File is empty,Please attach Image.", null, StatusCodes.Status400BadRequest, null);
            }
            UploadImageResponse resultResponse = new();
            var user = await _userManager.FindByIdAsync(upload.Id);
            if (user == null)
            {
                return new ApiResponse<UploadImageResponse>(false, "User not found.", null, StatusCodes.Status404NotFound, null);
            }

            using (var memoryStream = new MemoryStream())
            {
                await upload.formFile.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();
                var base64String = Convert.ToBase64String(imageBytes);
                var profilePictureUrl = $"data:{upload.formFile.ContentType};base64,{base64String}";
                user.ProfilePictureUrl = profilePictureUrl;
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded) { return new ApiResponse<UploadImageResponse>(false, "Failed to update Image. Please try again or contact support if the problem persists.", null, StatusCodes.Status400BadRequest, null); }
                resultResponse.ProfilePictureUrl = profilePictureUrl;
                return new ApiResponse<UploadImageResponse>(true, "Image Uploaded Successfully", resultResponse, StatusCodes.Status200OK, null);
            }
        }
    }
}
