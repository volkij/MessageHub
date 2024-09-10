using System.Text.Json.Serialization;

namespace MessageHub.Domain.DTO
{
    public class GeTemplateResponse
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        /*
        [JsonPropertyName("content")]
        public string Content { get; set; }
        */

        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        public DateTime DateMod { get; set; }
    }
}
