using NanhiDuniya.Services.AuthAPI.Models.Dto;

namespace NanhiDuniya.Services.AuthAPI.Service.IService
{
    public interface IImageService
    {
        Task<ApiResponse<UploadImageResponse>> SaveImageAsync(UploadProfilePictureDto uploadProfilePictureDto);

    }
}
