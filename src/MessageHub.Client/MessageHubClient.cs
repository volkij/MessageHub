using MessageHub.Client.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MessageHub.Client
{
    /// <summary>
    /// Client for sending messages to MessageHub
    /// </summary>
    public class MessageHubClient
    {
        private string _apiKey;
        private string _baseUrl = "http://10.0.5.4:5000";
        private string _serviceName;
        private int _version = 1;
        public MessageHubClient(string apiKey)
        {
            _apiKey = apiKey;
        }

        public MessageHubClient(string apiKey, string serviceName)
        {
            _apiKey = apiKey;
            _serviceName = serviceName;
        }

        public MessageHubClient(ClientProfile clientProfile)
        {
            LoadClientProfile(clientProfile);
        }

        public MessageHubClient(ClientConfiguration clientConfiguration)
        {
            var clientProfile = clientConfiguration.Profiles.FirstOrDefault();
            LoadClientProfile(clientProfile);
        }

        public MessageHubClient(ClientConfiguration clientConfiguration, string profileName)
        {
            var clientProfile = clientConfiguration.Profiles.FirstOrDefault(p => p.Name == profileName);
            LoadClientProfile(clientProfile);
        }

        private void LoadClientProfile(ClientProfile clientProfile)
        {
            _apiKey = clientProfile.ApiKey;
            _serviceName = clientProfile.ServiceName;

            if (clientProfile.Version > 0)
            {
                _version = clientProfile.Version;
            }

            if (!string.IsNullOrEmpty(clientProfile.Url))
            {
                _baseUrl = clientProfile.Url;
            }
        }

        public async Task<Response> SendSmsAsyn(SmsMessage message, string senderCode = null)
        {
            MessageRequest messageRequest = new MessageRequest()
            {
                type = "SMS",
                serviceName = _serviceName,
                priority = 1,
                contactValue = message.PhoneNumber,
                content = message.Message,
                templateCode = message.TemplateCode,
                expiration = message.ExpirationUtc,
                category = message.Category,
                senderCode = senderCode,
                contentAttributes = message.ContentAttributes?.Select(item => new ContentAttributeRequest
                {
                    name = item.Key,
                    value = item.Value
                }).ToList()
            };

            return await SendAsync(messageRequest, "/messages");
        }

        public async Task<Response> SendEmailAsyn(EmailMessage message, string senderCode = null)
        {
            MessageRequest messageRequest = new MessageRequest()
            {
                type = "EMAIL",
                serviceName = _serviceName,
                priority = 1,
                contactValue = message.EmailTo,
                subject = message.Subject,
                content = message.Body,
                templateCode = message.TemplateCode,
                expiration = message.ExpirationUtc,
                category = message.Category,
                senderCode = senderCode,
                contentAttributes = message.ContentAttributes?.Select(item => new ContentAttributeRequest
                {
                    name = item.Key,
                    value = item.Value
                }).ToList()
            };

            return await SendAsync(messageRequest, "/messages");
        }

        public async Task<Response> SendPushAsyn(PushMessage message, string senderCode = null)
        {
            MessageRequest messageRequest = new MessageRequest()
            {
                type = "PUSH",
                serviceName = _serviceName,
                priority = 1,
                subject = message.Title,
                content = message.Body,
                externalClientID = message.ClientId,
                templateCode = message.TemplateCode,
                expiration = message.ExpirationUtc,
                category = message.Category,
                senderCode = senderCode,
                contentAttributes = message.ContentAttributes?.Select(item => new ContentAttributeRequest
                {
                    name = item.Key,
                    value = item.Value
                }).ToList()
            };

            return await SendAsync(messageRequest, "/messages");
        }



        public async Task<Response> CreatePushToken(string externalClientId, string tokenValue)
        {
            var pushTokenCreateRequest = new PushTokenCreateRequest
            {
                externalClientID = externalClientId,
                value = tokenValue
            };

            return await SendAsync(pushTokenCreateRequest, "/pushtokens");
        }

        private static readonly HttpClient client = new HttpClient();

        private async Task<Response> SendAsync<T>(T requestObject, string endpoint)
        {
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("X-API-Key", _apiKey);

            var jsonContent = JsonConvert.SerializeObject(requestObject);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var apiResponse = await client.PostAsync($"{_baseUrl}/api/v{_version}{endpoint}", content);

                return new Response
                {
                    StatusCode = apiResponse.StatusCode,
                    ReasonPhrase = apiResponse.ReasonPhrase
                };
            }
            catch (HttpRequestException ex)
            {
                return new Response
                {
                    StatusCode = HttpStatusCode.ServiceUnavailable,
                    ReasonPhrase = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    ReasonPhrase = ex.Message
                };
            }
        }

        public async Task<List<Message>> GetMessagesUnreadAsync(MessageType messageType, string clientID)
        {
            var list = await GetMessagesAsync(messageType, clientID);
            return list.Where(m => m.ReadDate.HasValue == false).ToList();
        }


        public async Task<List<Message>> GetMessagesAsync(MessageType messageType, string clientID)
        {
            string url = _baseUrl + $"/api/v{_version}/messages";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-Key", _apiKey);

                // Odeslání GET požadavku
                var apiResponse = await client.GetAsync(_baseUrl + $"/api/v{_version}/messages?messageType={messageType}&externalClientId={clientID}");


                if (!apiResponse.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Request failed with status code: {apiResponse.StatusCode}");
                }

                var jsonResponse = await apiResponse.Content.ReadAsStringAsync();
                var messages = JsonConvert.DeserializeObject<List<Message>>(jsonResponse);

                return messages;
            }
        }

        public async Task<Response> MarkMessageAsRead(int messageID)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("X-API-Key", _apiKey);

                // Odeslání PUT požadavku
                var apiResponse = await client.PutAsync(_baseUrl + $"/api/v{_version}/messages/markasread/{messageID}", null);

                Response response = new Response() { StatusCode = apiResponse.StatusCode };
                return response;
            }
        }

    }
}