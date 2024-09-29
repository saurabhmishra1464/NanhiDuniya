using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Contracts
{
    public class SendEmailEvent
    {
        public List<string> ToEmail { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string HtmlBody { get; set; }

    }
}
