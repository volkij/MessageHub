
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MessageHub.Providers.SendGrid
{
    public class SendGridEmailService : MessageHub.Shared.IProvider<MessageHub.Shared.EmailMessage>
    {
        protected ILogger Logger;
        protected MessageHub.Shared.SenderConfig _senderConfig;
        private SendGridClient _client;

        public MessageHub.Shared.MessageType MessageType => MessageHub.Shared.MessageType.EMAIL;

        public SendGridEmailService(ILogger logger, MessageHub.Shared.SenderConfig senderConfig)
        {
            Logger = logger;
            _senderConfig = senderConfig;
            _client = new SendGridClient(_senderConfig.ApiKey);
        }

        public async Task<MessageHub.Shared.ProviderResult> SendAsync(MessageHub.Shared.EmailMessage message)
        {
            Shared.ProviderResult result = new Shared.ProviderResult();
            try
            {
                var from = new EmailAddress(message.From.Email, message.From.Name);
                var to = new EmailAddress(message.To.Email, message.To.Name);

                var msg = MailHelper.CreateSingleEmail(from, to, message.Subject, message.BodyPlain, message.BodyHtml);

                // Odeslání e-mailu
                Logger.LogDebug($"Sending email to SendGrid");
                var response = await _client.SendEmailAsync(msg);

                Logger.LogDebug($"Response from SendGrid: {response.StatusCode}");

                if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
                {
                    result.IsSuccess = false;
                    result.Message = response.Body.ReadAsStringAsync().Result;
                }
                else
                {
                    result.IsSuccess = true;
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