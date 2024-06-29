﻿namespace NanhiDuniya.Services.EmailApi.Services
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
