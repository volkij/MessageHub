using MessageHub.Shared;

namespace MessageHub.Domain.Events
{
    /// <summary>
    /// Event that is triggered when an Push is queued for sending
    /// </summary>
    public class PushQueuedEvent
    {
        public PushQueuedEvent(PushMessage pushMessage, int messageId, string senderCode)
        {
            PushMessage = pushMessage;
            MessageId = messageId;
            SenderCode = senderCode;
        }

        #region Properties
        public PushMessage PushMessage { get; set; }

        public int MessageId { get; set; }

        public string SenderCode { get; set; }

        #endregion
    }
}