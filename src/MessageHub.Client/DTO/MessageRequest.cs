
using System;
using System.Collections.Generic;

namespace MessageHub.Client.DTO
{
    internal class MessageRequest
    {
        public string type { get; set; }

        public string contactValue { get; set; }

        public string subject { get; set; }

        public string templateCode { get; set; }

        public string campaignCode { get; set; }

        public string serviceName { get; set; }

        public string externalMessageID { get; set; }

        public string content { get; set; }

        public DateTime? expiration { get; set; }

        public int? priority { get; set; }

        public string category { get; set; }

        public string senderCode { get; set; }

        public string externalClientID { get; set; }

        public List<ContentAttributeRequest> contentAttributes { get; set; }
    }
}