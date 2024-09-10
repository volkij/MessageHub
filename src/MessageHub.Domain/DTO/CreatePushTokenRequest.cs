using System.Text.Json.Serialization;

namespace MessageHub.Domain.DTO
{
    /// <summary>
    /// Request to create a push token
    /// </summary>
    public class CreatePushTokenRequest
    {
        [JsonPropertyName("externalClientID")]
        public required string ExternalClientID { get; set; }

        [JsonPropertyName("value")]
        public required string Value { get; set; }
    }
}
