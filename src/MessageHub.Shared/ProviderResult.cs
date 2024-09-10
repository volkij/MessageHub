namespace MessageHub.Shared
{
    public class ProviderResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public List<string> UndeliveryContacts = new List<string>();
    }
}
