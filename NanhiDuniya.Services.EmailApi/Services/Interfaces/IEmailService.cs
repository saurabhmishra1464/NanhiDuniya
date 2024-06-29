using NanhiDuniya.Services.EmailApi.Models;

namespace NanhiDuniya.Services.EmailApi.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(EmailRequest request);
    }
}
