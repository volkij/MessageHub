namespace MessageHub.Shared
{
    public class PushMessage
    {
        public PushMessage(string title, string body, List<string> tokens, string externalMessageID)
        {
            Title = title;
            Body = body;
            Tokens = tokens;
            ExternalMessageID = externalMessageID;
        }


        public string Title { get; set; }
        public string Body { get; set; }

        public string ExternalMessageID { get; set; }

        public List<string> Tokens { get; set; }
    }
}
