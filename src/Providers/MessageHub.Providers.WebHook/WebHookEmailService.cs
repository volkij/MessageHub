using MessageHub.Shared;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace MessageHub.Providers.WebHook
{
    public class WebHookEmailService : MessageHub.Shared.IProvider<MessageHub.Shared.EmailMessage>
    {
        protected ILogger Logger;
        protected MessageHub.Shared.SenderConfig _senderConfig;

        private static readonly HttpClient client = new HttpClient();

        public MessageHub.Shared.MessageType MessageType => MessageHub.Shared.MessageType.EMAIL;


        public WebHookEmailService(ILogger logger, MessageHub.Shared.SenderConfig senderConfig)
        {
            Logger = logger;
            _senderConfig = senderConfig;

        }

        public async Task<ProviderResult> SendAsync(EmailMessage message)
        {
            Shared.ProviderResult result = new Shared.ProviderResult();

            try
            {
                var json = JsonSerializer.Serialize(message);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Logger.LogDebug($"Sending POST to WebHook");

                var response = client.PostAsync(_senderConfig.Url, content).Result;

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    result.IsSuccess = false;
                }
                else
                {
                    result.IsSuccess = true;
                }

                Logger.LogDebug($"Response from WebHook: {response.StatusCode}");


            }
            catch (System.Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }
    }
}