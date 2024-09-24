using System.Text.Json.Serialization;

namespace MessageHub.Domain.DTO
{
    /// <summary>
    /// Request for creating a message
    /// </summary>
    public class CreateMessageRequest
    {
        [JsonPropertyName("type")]
        public required string Type { get; set; }

        [JsonPropertyName("contactValue")]
        public string? ContactValue { get; set; }

        [JsonPropertyName("subject")]
        public string? Subject { get; set; }

        [JsonPropertyName("templateCode")]
        public string? TemplateCode { get; set; }

        [JsonPropertyName("campaignCode")]
        public string? CampaignCode { get; set; }

        [JsonPropertyName("serviceName")]
        public string? ServiceName { get; set; }

        [JsonPropertyName("externalMessageID")]
        public string? ExternalMessageID { get; set; }

        [JsonPropertyName("externalClientID")]
        public string? ExternalClientID { get; set; }

        [JsonPropertyName("content")]
        public string? Content { get; set; }

        [JsonPropertyName("expiration")]
        public DateTime? Expiration { get; set; }

        [JsonPropertyName("priority")]
        public int? Priority { get; set; }

        [JsonPropertyName("category")]
        public string? Category { get; set; }

        [JsonPropertyName("senderCode")]
        public string? SenderCode { get; set; }

        [JsonPropertyName("tags")]
        public string[]? Tags { get; set; }

        [JsonPropertyName("attachments")]
        public List<MessageAttachment>? Attachments { get; set; }

        [JsonPropertyName("contentAttributes")]
        public List<MessageContentAttribute>? ContentAttributes { get; set; }
    }
}