using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Email.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailService> _logger;
        public EmailService(ILogger<EmailService> logger, IOptions<EmailSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task<bool> SendEmailAsync(string subject, string? htmlBody = null, string? plainTextBody = null, string? templateName = null, string? to = null, List<string>? toList = null)
        {
            try
            {
                if (toList == null || toList.Count == 0)
                {
                    if (string.IsNullOrWhiteSpace(to))
                    {
                        throw new ArgumentException("Either to or toList must be specified");
                    }
                    toList ??= new List<string>();
                    toList.Add(to);
                }
                var request = new NanhiDuniyaEmailRequest()
                {
                    From = _settings.DefaultFrom,
                    TemplateName = templateName,
                    Subject = subject,
                    HtmlBody = htmlBody,
                    PlainTextBody = plainTextBody,
                    ToList = toList
                };
                return await SendEmailAsync(request);
            }
            catch (Exception ex)
            {
                // Log the exception and rethrow or handle as needed
                throw new InvalidOperationException("Failed to send email.", ex);
            }

        }

        private void AddAttachments(NanhiDuniyaEmailRequest request, BodyBuilder bodyBuilder)
        {
            if (request.Attachments != null && request.Attachments.Any())
            {
                foreach (var attachment in request.Attachments)
                {
                    try
                    {
                        byte[] fileBytes = Convert.FromBase64String(attachment.Base64Content);
                        bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                    }
                    catch (FormatException ex)
                    {
                        // Log or handle the base64 decoding error
                        Console.WriteLine($"Error decoding base64 content for {attachment.FileName}: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        // Log or handle other errors
                        Console.WriteLine($"Error adding attachment {attachment.FileName}: {ex.Message}");
                    }
                }
            }
        }

        public async Task<bool> SendEmailAsync(NanhiDuniyaEmailRequest request)
        {
            try
            {
                MimeMessage message = CreateEmailMessage(request);
                //String result = await _smtpClient.SendAsync(message);
                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, false);
                    await client.AuthenticateAsync(_settings.Username, _settings.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email.");
                return false;
            }
        }

        //private MimeMessage CreateMessage(NanhiDuniyaEmailRequest request)
        //{
        //    if (string.IsNullOrWhiteSpace(request.From))
        //    {
        //        request.From = _settings.DefaultFrom;
        //    }
        //    var content = JsonSerializer.Serialize<NanhiDuniyaEmailRequest>(request);
        //    HttpContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");
        //    HttpRequestMessage message = new HttpRequestMessage()
        //    {
        //        Method = HttpMethod.Post,
        //        RequestUri = new Uri(_serviceUrl),
        //        Content = httpContent
        //    };
        //    return message;
        //}

        private MimeMessage CreateEmailMessage(NanhiDuniyaEmailRequest request)
        {
            var emailMessage = new MimeMessage();
            try
            {
                emailMessage.From.Add(new MailboxAddress("NanhiDuniya", _settings.DefaultFrom));
                //var mailboxaddress = message.ToList.Select(address => new MailboxAddress(address)).ToList();
                var mailboxAddresses = request.ToList.Select(address => new MailboxAddress(string.Empty, address)).ToList();
                emailMessage.To.AddRange(mailboxAddresses);
                emailMessage.Subject = request.Subject;

                var bodyBuilder = new BodyBuilder { HtmlBody = string.Format("<h2 style='color:red;'>{0}</h2>", request) };

                // Add attachments using a helper method
                AddAttachments(request, bodyBuilder);

                emailMessage.Body = bodyBuilder.ToMessageBody();

            }
            catch (Exception ex)
            {

            }

            return emailMessage;
        }
    }
}
