using NanhiDuniya.MessageBus;

namespace NanhiDuniya.Services.EmailApi.Entities
{
    public class SendEmailItem
    {
        public Guid Id { get; set; }
        public List<string> ToEmail { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }
    }
}
