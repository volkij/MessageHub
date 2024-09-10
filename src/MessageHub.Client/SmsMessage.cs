using System;
using System.Collections.Generic;

namespace MessageHub.Client
{
    /// <summary>
    /// Structure for SMS message
    /// </summary>
    public class SmsMessage
    {
        public string PhoneNumber { get; set; }
        public string Message { get; set; }

        public string TemplateCode { get; set; }

        public DateTime? ExpirationUtc { get; set; }

        public string Category { get; set; }

        public Dictionary<string, string> ContentAttributes { get; set; }
    }
}