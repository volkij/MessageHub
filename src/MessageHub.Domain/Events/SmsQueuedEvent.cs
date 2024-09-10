using MessageHub.Shared;

namespace MessageHub.Domain.Events
{
    /// <summary>
    /// Event that is triggered when an sms is queued for sending
    /// </summary>
    public class SmsQueuedEvent
    {
        public SmsQueuedEvent(SmsMessage smsMessage, int messageId, string senderCode)
        {
            SmsMessage = smsMessage;
            MessageId = messageId;
            SenderCode = senderCode;
        }

        #region Properties
        public SmsMessage SmsMessage { get; set; }

        public int MessageId { get; set; }

        public string SenderCode { get; set; }

        #endregion
    }
}