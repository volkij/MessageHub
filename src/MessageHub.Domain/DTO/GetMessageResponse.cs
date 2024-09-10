using System.Text.Json.Serialization;

namespace MessageHub.Domain.DTO
{
    public class GetMessageResponse
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("externalClientID")]
        public string? ExternalClientID { get; set; }

        [JsonPropertyName("subject")]
        public string? Subject { get; set; }

        [JsonPropertyName("content")]
        public string? Content { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("sentDate")]
        public DateTime? SentDate { get; set; }

        [JsonPropertyName("readDate")]
        public DateTime? ReadDate { get; set; }

    }
}
