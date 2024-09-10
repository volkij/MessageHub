using MessageHub.Domain.Enums;
using MessageHub.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MessageHub.Domain.Entities
{
    public class Message : Entity
    {
        public Message()
        {
            Attachments = new List<MessageAttachment>();
            ContentAttributes = new List<MessageAttribute>();
        }

        public string? ContactValue { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? Subject { get; set; }
        public string? TemplateCode { get; set; }
        public string? CampaignCode { get; set; }
        public string? ExternalMessageID { get; set; }
        public string? Content { get; set; }
        public DateTime? Expiration { get; set; }

        [Required]
        public int Priority { get; set; }
        public string? Category { get; set; }
        public string[]? Tags { get; set; }

        private string _type;

        [NotMapped]
        public MessageType MessageType
        {
            get => Enum.TryParse<MessageType>(_type, out var type) ? type : default;
            set => _type = value.ToString();
        }

        [Required]
        public string Type
        {
            get => _type;
            private set => _type = value;
        }

        private string _status;

        [NotMapped]
        public MessageStatus MessageStatus
        {
            get => Enum.TryParse<MessageStatus>(_status, out var status) ? status : default;
            set => _status = value.ToString();
        }

        [Required]
        public string Status
        {
            get => _status;
            private set => _status = value;
        }

        public DateTime? SentDate { get; set; }
        public string? ServiceName { get; set; }
        public virtual ICollection<MessageAttachment> Attachments { get; set; }
        public virtual ICollection<MessageAttribute> ContentAttributes { get; set; }
        public string Account { get; set; }
        public string? SenderCode { get; set; }
        public string? ExternalClientID { get; set; }
        public DateTime? ReadDate { get; set; }

    }
}