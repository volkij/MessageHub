using System;
using System.Collections.Generic;

namespace MessageHub.Client.Messages
{
    public static class SmsMessageHelper
    {
        public static SmsMessage CreateSingleSmsMessage(string phoneNumber, string message,
            DateTime? expirationUtc = null, string category = null, Dictionary<string, string> contentAttributes = null)
        {
            return new SmsMessage()
            {
                PhoneNumber = phoneNumber,
                Message = message,
                ExpirationUtc = expirationUtc,
                Category = category,
                ContentAttributes = contentAttributes
            };
        }

        public static SmsMessage CreateSingleSmsMessageFromTemplate(string phoneNumber, string templateCode,
            DateTime? expirationUtc = null, string category = null, Dictionary<string, string> contentAttributes = null)
        {
            return new SmsMessage()
            {
                PhoneNumber = phoneNumber,
                TemplateCode = templateCode,
                ExpirationUtc = expirationUtc,
                Category = category,
                ContentAttributes = contentAttributes
            };
        }
    }
}