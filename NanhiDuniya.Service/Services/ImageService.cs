using Microsoft.AspNetCore.Http;
using NanhiDuniya.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Service.Services
{
    public class ImageService : IImageService
    {
        public async Task<string> SaveImageAsync(IFormFile image, string uploadDirectory)
        {
            if (image == null || image.Length == 0)
            {
                throw new ArgumentNullException(nameof(image));
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(uploadDirectory, fileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}
