using MessageHub.Shared;

namespace MessageHub.Domain.Events
{
    /// <summary>
    /// Event that is triggered when an email is queued for sending
    /// </summary>
    public class EmailQueuedEvent
    {
        public EmailQueuedEvent(EmailMessage emailMessage, int messageId, string senderCode)
        {
            EmailMessage = emailMessage;
            MessageId = messageId;
            SenderCode = senderCode;
        }

        #region Properties

        public EmailMessage EmailMessage { get; set; }

        public int MessageId { get; set; }

        public string SenderCode { get; set; }

        #endregion
    }
}