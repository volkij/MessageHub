namespace MessageHub.Infrastructure.ServiceBus
{
    public static class Exchanges
    {
        public const string Messages = "messages";
        public const string MessagesNew = "messages-new";

        public const string Emails = "emails";
        public const string EmailsQueued = "emails-queued";

        public const string Sms = "sms";
        public const string SmsQueued = "sms-queued";

        public const string Push = "push";
        public const string PushQueued = "push-queued";

        public const string ProviderResult = "providerresult";
        public const string ProviderResultNew = "providerresult-new";
    }


}
