using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Core.Resources
{
    public class NanhiDuniyaEmailRequest
    {
        public List<string> ToEmail { get; set; }
        public string From { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string TemplateName { get; set; } = null!;
        public string HtmlBody { get; set; } = null!;
    }
}
