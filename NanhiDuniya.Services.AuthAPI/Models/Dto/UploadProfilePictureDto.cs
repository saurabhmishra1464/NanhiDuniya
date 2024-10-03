namespace NanhiDuniya.Services.AuthAPI.Models.Dto
{
    public class UploadProfilePictureDto
    {
        public string Id { get; set; }
        public IFormFile formFile { get; set; }
    }
}
