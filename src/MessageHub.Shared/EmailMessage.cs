namespace MessageHub.Shared
{
    public class EmailMessage : BaseProviderMessage
    {
        public EmailMessage(EmailAddress from, EmailAddress to, string subject, string bodyPlain, string bodyHtml)
        {
            From = from;
            To = to;
            Subject = subject;
            BodyPlain = bodyPlain;
            BodyHtml = bodyHtml;
        }

        public EmailAddress From { get; set; }
        public EmailAddress To { get; set; }
        public string Subject { get; set; }
        public string BodyPlain { get; set; }
        public string BodyHtml { get; set; }
    }
}
