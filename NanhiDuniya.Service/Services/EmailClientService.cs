using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NanhiDuniya.Core.Interfaces;
using NanhiDuniya.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace NanhiDuniya.Service.Services
{
    public class EmailClientService : IEmailClientService
    {
        private readonly string _serviceUrl = null!;
        private readonly HttpClient _httpClient = null!;
        private readonly string _defaultFromAddress = null!;
        private readonly NanhiDuniyaServicesSettings _nanhiDuniyaSettings;
        private readonly IHostEnvironment _env;

        public EmailClientService(IServiceProvider serviceProvider, IHostEnvironment env,
    IOptions<NanhiDuniyaServicesSettings> nanhiduniyaSettingsOpt)
        {
            _httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();
            _nanhiDuniyaSettings = nanhiduniyaSettingsOpt?.Value ?? throw new ArgumentNullException(nameof(nanhiduniyaSettingsOpt));
            _serviceUrl = _nanhiDuniyaSettings.Email.SendMailUrl;
            _defaultFromAddress = _nanhiDuniyaSettings.Email.DefaultFrom;
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        private string LoadHtmlTemplate(string templatePath)
        {
            ValidateFileExists(templatePath);
            string htmlContent = File.ReadAllText(templatePath);
            htmlContent = htmlContent.Replace("{{SchoolName}}", "Nanhi Duniya");
            return htmlContent;
        }

        private void ValidateFileExists(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}", filePath);
        }

        public async Task<bool> SendEmailAsync(string subject, string? htmlBody = null, string? plainTextBody = null, string? templateName = null, string? to = null, List<string>? toList = null)
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
            var templatePath = Path.Combine(_env.ContentRootPath, "Templates", $"{templateName}.html");

            htmlBody = LoadHtmlTemplate(templatePath);

            var request = new NanhiDuniyaEmailRequest()
            {
                ToList = toList,
                CcList = new List<string> { "ccuser@example.com" }, // Add CC addresses if needed
                From = _defaultFromAddress,
                Subject = subject,
                TemplateName = templateName,
                HtmlBody = htmlBody,
            };


            return await SendEmailAsync(request);
        }

        public async Task<bool> SendEmailAsync(NanhiDuniyaEmailRequest request)
        {
            try
            {
                var response = await _httpClient.SendAsync(CreateMessage(request));

                response.EnsureSuccessStatusCode();
                Console.WriteLine(response);
                return true;
            }
            catch (HttpRequestException httpRequestException)
            {
                // Handle HTTP request specific errors
            }
            catch (Exception ex)
            {
                // Handle other error
            }
            return false;
        }

        private HttpRequestMessage CreateMessage(NanhiDuniyaEmailRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.From))
            {
                request.From = _defaultFromAddress;
            }
            var content = JsonSerializer.Serialize<NanhiDuniyaEmailRequest>(request);
            HttpContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");
            HttpRequestMessage message = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_serviceUrl),
                Content = httpContent
            };
            return message;
        }
    }
}
