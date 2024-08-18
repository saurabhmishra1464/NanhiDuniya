using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NanhiDuniya.Core.Interfaces;
using NanhiDuniya.Core.Models;
using NanhiDuniya.Core.Resources.AccountDtos;
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
        public ImageService(UserManager<ApplicationUser> userManager) 
        { 
        _userManager = userManager;
        }
        public async Task<ResultResponse> SaveImageAsync(UploadProfilePictureDto image/*, string uploadDirectory*/)
        {
            ResultResponse resultResponse = new();
            if (image.formFile == null || image.formFile.Length == 0)
            {
                throw new ArgumentNullException(nameof(image));
            }
            var user = await _userManager.FindByIdAsync(image.Id);
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.formFile.FileName);
            user.ProfilePictureUrl = fileName;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                resultResponse.Message = "Image Uploaded Uploaded Successfully";
                resultResponse.IsSuccess = true;
            }
            else
            {
                resultResponse.Message = "Something went wrong while uploading Image";
            }
            //var filePath = Path.Combine(uploadDirectory, fileName);

            //await using (var stream = new FileStream(filePath, FileMode.Create))
            //{
            //    await image.CopyToAsync(stream);
            //}

            return resultResponse; ;
        }
    }
}
