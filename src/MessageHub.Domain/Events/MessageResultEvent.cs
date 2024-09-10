using MessageHub.Shared;

namespace MessageHub.Domain.Events
{
    /// <summary>
    /// Event that is triggered when a message result is received
    /// </summary>
    public class ProviderResultEvent
    {
        public ProviderResultEvent(ProviderResult providerResult, int messageId)
        {
            ProviderResult = providerResult;
            MessageId = messageId;
        }

        #region Properties

        public int MessageId { get; set; }
        public ProviderResult ProviderResult { get; set; }

        #endregion
    }
}