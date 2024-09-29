using MassTransit;
using NanhiDuniya.Contracts;
using NanhiDuniya.MessageBus;
using NanhiDuniya.MessageBus.SQL;
using NanhiDuniya.Services.EmailApi.Entities;
using NanhiDuniya.Services.EmailApi.Models;
using NanhiDuniya.Services.EmailApi.Services.Interfaces;

namespace NanhiDuniya.Services.EmailApi.Consumers
{
    public class SendEmailConsumer : IConsumer<SendEmailEvent>
    {
        private readonly IEmailService _emailService;
        public SendEmailConsumer(IEmailService emailService)
        {
            _emailService = emailService;
        }
        public async Task Consume(ConsumeContext<SendEmailEvent> context)
        {
            var message = context.Message;
            var emailRequest = new EmailRequest
            {
                ToList = message.ToEmail,
                From = message.From,
                Subject = message.Subject,
                HtmlBody = message.HtmlBody,
            };
           await _emailService.SendEmailAsync(emailRequest);
        }
    }
}
