namespace MessageHub.Domain.Exceptions
{
    public class NotFoundException : MessageHubException
    {
        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public NotFoundException(string message, object key, object value)
            : base(message, key, value)
        {
        }
    }
}