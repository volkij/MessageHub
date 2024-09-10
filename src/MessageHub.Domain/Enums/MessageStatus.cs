namespace MessageHub.Domain.Enums
{
    public enum MessageStatus
    {
        Created,
        Queued,
        Sent,
        Finished,
        SentFailed,
        Expired,
        Failed
    }
}