using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Core.Resources.AccountDtos
{
    public class UploadProfilePictureDto
    {
        public string Id { get; set; }
        public IFormFile formFile { get; set; }
    }
}
