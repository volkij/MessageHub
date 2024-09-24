using System;

namespace MessageHub.Client.Messages
{
    public class Message
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string ExternalClientID { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }

        public string Category { get; set; }

        public DateTime? SentDate { get; set; }
        public DateTime? ReadDate { get; set; }
    }
}
