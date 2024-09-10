using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using MessageHub.Shared;
using Microsoft.Extensions.Logging;

namespace MessageHub.Providers.Firebase
{
    public class FirebasePushService : IProvider<MessageHub.Shared.PushMessage>
    {
        protected ILogger Logger;
        protected MessageHub.Shared.SenderConfig _senderConfig;

        private static readonly HttpClient client = new HttpClient();
        public MessageHub.Shared.MessageType MessageType => MessageHub.Shared.MessageType.PUSH;

        private FirebaseApp FirebaseApp { get; set; }

        public FirebasePushService(ILogger logger, MessageHub.Shared.SenderConfig senderConfig)
        {
            Logger = logger;
            _senderConfig = senderConfig;

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Storage", "Firebase", senderConfig.ConfigFileName);

            FirebaseApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(filePath),
            }, senderConfig.Code);

        }

        public async Task<ProviderResult> SendAsync(MessageHub.Shared.PushMessage message)
        {


            Shared.ProviderResult result = new Shared.ProviderResult();

            try
            {
                string[] tokens = message.Tokens.ToArray();

                var multiMessage = new MulticastMessage()
                {
                    Tokens = tokens,
                    Notification = new Notification
                    {
                        Title = message.Title,
                        Body = message.Body,
                    },
                };

                Logger.LogDebug($"Sending Multicast message to Firebase");
                BatchResponse response = await FirebaseMessaging.GetMessaging(FirebaseApp).SendMulticastAsync(multiMessage);
                Logger.LogDebug($"{response.SuccessCount} messages were sent successfully");
                Logger.LogDebug($"{response.FailureCount} messages failed to send");

                for (int i = 0; i < response.Responses.Count; i++)
                {
                    var firebaseResult = response.Responses[i];
                    var token = tokens[i];

                    if (firebaseResult.IsSuccess)
                    {
                        Logger.LogDebug("Successfully sent message to token: " + firebaseResult.MessageId);
                    }
                    else
                    {

                        Logger.LogDebug("Failed to send message to token: " + firebaseResult.Exception.Message);
                        result.UndeliveryContacts.Add(token);

                    }
                }

                if (response.SuccessCount > 0)
                {
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                }

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
