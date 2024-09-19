using Microsoft.AspNetCore.Http;
using NanhiDuniya.Core.Models;
using NanhiDuniya.Core.Resources.AccountDtos;
using NanhiDuniya.Core.Resources.ResponseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Core.Interfaces
{
    public interface IImageService
    {
        Task<ApiResponse<UploadImageResponse>> SaveImageAsync(UploadProfilePictureDto uploadProfilePictureDto);

    }
}
