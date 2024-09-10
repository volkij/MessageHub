using MessageHub.Domain.DTO;

namespace MessageHub.Domain.Events
{
    /// <summary>
    /// Event that is triggered when a message is created
    /// </summary>
    public class MessageCreateEvent
    {
        public MessageCreateEvent(CreateMessageRequest createMessageRequest, string accountName)
        {
            CreateMessageRequest = createMessageRequest;
            AccountName = accountName;
        }

        #region Properties

        public CreateMessageRequest CreateMessageRequest { get; set; }
        public string AccountName { get; set; }

        #endregion
    }
}