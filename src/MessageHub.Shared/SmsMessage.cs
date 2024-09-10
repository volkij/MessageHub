namespace MessageHub.Shared
{
    public class SmsMessage : BaseProviderMessage
    {
        public SmsMessage(string sender, string phoneNumber, string text)
        {
            Sender = sender;
            PhoneNumber = phoneNumber;
            Text = text;
        }

        public string Sender { get; set; }
        public string PhoneNumber { get; set; }
        public string Text { get; set; }
    }
}