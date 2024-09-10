namespace MessageHub.Domain.Exceptions
{
    public class MessageHubException : Exception
    {
        public MessageHubException()
        {
        }

        public MessageHubException(string message)
            : base(message)
        {
        }

        public MessageHubException(string message, object key, object value)
            : base(message)
        {
            Data.Add(key, value);
        }

        public MessageHubException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}