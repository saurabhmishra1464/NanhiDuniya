using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Service.Services
{
    public class EmailServiceSettings
    {
        public string? DefaultFrom { get; set; }
        public string? SendMailUrl { get; set; }
    }

    public class NanhiDuniyaServicesSettings
    {
        public EmailServiceSettings? Email { get; set; }
        public string? ClientBaseUrl { get; set; }
        public string? DataServiceBaseUrl { get; set; }
    }
}
