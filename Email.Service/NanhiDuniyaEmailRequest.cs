using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Email.Service
{
    public class NanhiDuniyaEmailRequest
    {
        public string RequestingModule { get; set; } = null!;
        public List<string> ToList { get; set; } = new();
        public List<string> CcList { get; set; } = new();
        public List<string> BccList { get; set; } = new();
        public string From { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string TemplateName { get; set; } = null!;
        public List<string> SupportTemplates { get; set; } = new();
        public List<string> SupportDlls { get; set; } = new();
        public string ModelType { get; set; } = null!;
        public string ModelData { get; set; } = null!;
        public string HtmlBody { get; set; } = null!;
        public string PlainTextBody { get; set; } = null!;
        public List<NanhiDuniyaEmailAttachment> Attachments { get; set; } = new();
    }

    public class NanhiDuniyaEmailAttachment
    {
        public string FileName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public string Base64Content { get; set; } = null!;
    }
}
