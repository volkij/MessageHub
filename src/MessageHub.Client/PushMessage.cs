using System;
using System.Collections.Generic;

namespace MessageHub.Client
{
    public class PushMessage
    {
        public string ClientId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public string TemplateCode { get; set; }

        public DateTime? ExpirationUtc { get; set; }

        public string Category { get; set; }

        public Dictionary<string, string> ContentAttributes { get; set; }

        public string ExternalMessageID { get; set; }
    }
}
