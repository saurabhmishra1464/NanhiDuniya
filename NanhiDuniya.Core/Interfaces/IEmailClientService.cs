using NanhiDuniya.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Core.Interfaces
{
    public  interface IEmailClientService
    {
        Task<bool> SendEmailAsync(string subject, string? htmlBody = null, string? plainTextBody = null, string? templateName = null, string? to = null, List<string>? toList = null);
        Task<bool> SendEmailAsync(NanhiDuniyaEmailRequest request);
    }
}
