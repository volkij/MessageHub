using System.Text.Json.Serialization;

namespace MessageHub.Domain.DTO
{
    public class MessageAttachment
    {
        [JsonPropertyName("fileName")]
        public required string FileName { get; set; }

        [JsonPropertyName("fileContent")]
        public required string FileContent { get; set; }
    }
}