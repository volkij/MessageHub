using MessageHub.Shared;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace MessageHub.Providers.EuroSms
{
    public class EuroSmsService : MessageHub.Shared.IProvider<MessageHub.Shared.SmsMessage>
    {
        protected ILogger Logger;
        protected MessageHub.Shared.SenderConfig _senderConfig;

        private static readonly HttpClient client = new HttpClient();

        public MessageHub.Shared.MessageType MessageType => MessageHub.Shared.MessageType.SMS;

        public EuroSmsService(ILogger logger, MessageHub.Shared.SenderConfig senderConfig)
        {
            Logger = logger;
            _senderConfig = senderConfig;

        }

        public async Task<ProviderResult> SendAsync(SmsMessage message)
        {
            var result = new ProviderResult();

            try
            {
                using var httpClient = new HttpClient();
                Logger.LogDebug("Sending POST to EuroSMS");

                var response = await httpClient.PostAsync(_senderConfig.Url, this.CreateEuroSMSContent(message));
                var responseString = await response.Content.ReadAsStringAsync();

                Logger.LogDebug($"Response from EuroSMS: Status code: {response.StatusCode}, {responseString}");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var xmlResponse = XDocument.Parse(responseString);
                    var errorElement = xmlResponse.Root.Element("error");

                    if (errorElement != null && errorElement.Value == "ok")
                    {
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = xmlResponse.Root.Element("errorDescription")?.Value ?? "Unknown error";
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = $"HTTP Error: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }

        private HttpContent CreateEuroSMSContent(SmsMessage message)
        {
            var xmlBuilder = new StringBuilder();
            xmlBuilder.Append(@"<?xml version=""1.0"" encoding=""UTF-8""?>")
                      .Append("<sms ")
                      .Append($@"i=""{_senderConfig.UserName}"" ")
                      .Append($@"s=""{CreateSignature(_senderConfig.Password, message.PhoneNumber)}"">")
                      .Append($"<sender>{message.Sender}</sender>")
                      .Append($"<number>{message.PhoneNumber}</number>")
                      .Append($"<msg>{message.Text}</msg>")
                      .Append("</sms>");

            var xml = xmlBuilder.ToString();

            var data = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("action", "send1SMS"),
                    new KeyValuePair<string, string>("xml", xml)
                };

            return new FormUrlEncodedContent(data);
        }

        private string CreateSignature(string integrationKey, string phone)
        {
            string source = integrationKey + phone;

            using (var md5Hash = MD5.Create())
            {
                string hash = GetMd5Hash(md5Hash, source);
                return hash.Length >= 21 ? hash.Substring(10, 11) : string.Empty;
            }
        }

        private string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
