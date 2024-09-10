namespace MessageHub.Client.DTO
{
    public class PushTokenCreateRequest
    {

        public string externalClientID { get; set; }


        public string value { get; set; }

        public string senderCode { get; set; }
    }
}
