using Microsoft.AspNetCore.Http;
using NanhiDuniya.Core.Models;
using NanhiDuniya.Core.Resources.AccountDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Core.Interfaces
{
    public interface IImageService
    {
        Task<ResultResponse> SaveImageAsync(UploadProfilePictureDto uploadProfilePictureDto/*, string uploadDirectory*/);

    }
}
