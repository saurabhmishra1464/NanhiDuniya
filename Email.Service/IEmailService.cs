using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email.Service
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string subject, string? htmlBody = null, string? plainTextBody = null, string? templateName = null, string? to = null, List<string>? toList = null);
    }
}
