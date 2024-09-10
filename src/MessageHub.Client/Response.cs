using System.Net;

namespace MessageHub.Client
{
    public class Response
    {
        public HttpStatusCode StatusCode { get; set; }

        public string ReasonPhrase { get; set; }
    }
}