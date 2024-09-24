using System;
using System.Collections.Generic;

namespace MessageHub.Client.Messages
{
    public static class PushMessageHelper
    {
        public static PushMessage CreateSinglePushMessage(string clientID, string title, string body,
            DateTime? expirationUtc = null, string category = null, Dictionary<string, string> contentAttributes = null)
        {
            return new PushMessage()
            {
                ClientId = clientID,
                Title = title,
                Body = body,
                ExpirationUtc = expirationUtc,
                Category = category,
                ContentAttributes = contentAttributes
            };
        }

        public static PushMessage CreateSinglePushMessage(string clientID, string templateCode,
            DateTime? expirationUtc = null, string category = null, Dictionary<string, string> contentAttributes = null)
        {
            return new PushMessage()
            {
                ClientId = clientID,
                TemplateCode = templateCode,
                ExpirationUtc = expirationUtc,
                Category = category,
                ContentAttributes = contentAttributes
            };
        }

    }
}
