using System;
using System.Collections.Generic;

namespace MessageHub.Client
{
    /// <summary>
    /// Structure for Email message
    /// </summary>
    public class EmailMessage
    {
        public string EmailTo { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public string TemplateCode { get; set; }

        public DateTime? ExpirationUtc { get; set; }

        public string Category { get; set; }

        public Dictionary<string, string> ContentAttributes { get; set; }
    }
}