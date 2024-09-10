using System.Text.Json.Serialization;

namespace MessageHub.Domain.DTO
{
    public class MessageContentAttribute
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}