using Microsoft.Extensions.Configuration;
using System.Diagnostics.Metrics;

namespace MessageHub.Services.Telemetry
{
    public class MessageHubMetrics
    {
        //Messages
        private Counter<int> MessageRequestCreateCounter { get; }
        private Counter<int> MessageRequestInvalidCounter { get; }
        private Counter<int> MessageByTypeCounter { get; }

        private Histogram<int> MessageRequest { get; }

        //PushTokens

        public MessageHubMetrics(IMeterFactory meterFactory, IConfiguration configuration)
        {
            var meter = meterFactory.Create("MessageHubMeter");

            MessageRequest = meter.CreateHistogram<int>("MessagesRequest", "1");

            MessageRequestCreateCounter = meter.CreateCounter<int>("messages-request-create", "MessageRequest");
            MessageRequestInvalidCounter = meter.CreateCounter<int>("messages-request-invalid", "MessageRequest");

            MessageByTypeCounter = meter.CreateCounter<int>("messages-type", "Message");
        }

        public void RecordMessageRequest() => MessageRequest.Record(1);
        public void MessageRequestCreate() => MessageRequestCreateCounter.Add(1);
        public void MessageRequestInvalid() => MessageRequestInvalidCounter.Add(1);
        public void MessageByType(string messageType) => MessageByTypeCounter.Add(1, KeyValuePair.Create<string, object>("MessageType", messageType));
    }
}
