using System;
using System.Collections.Generic;

namespace MessageHub.Client.Messages
{
    public static class EmailMessageHelper
    {
        public static EmailMessage CreateSingleEmailMessage(string emailTo, string subject, string body,
            DateTime? expirationUtc = null, string category = null, Dictionary<string, string> contentAttributes = null)
        {
            return new EmailMessage()
            {
                EmailTo = emailTo,
                Subject = subject,
                Body = body,
                ExpirationUtc = expirationUtc,
                Category = category,
                ContentAttributes = contentAttributes
            };
        }

        public static EmailMessage CreateSingleEmailMessageFromTemplate(string emailTo, string templateCode,
            DateTime? expirationUtc = null, string category = null, Dictionary<string, string> contentAttributes = null)
        {
            return new EmailMessage()
            {
                EmailTo = emailTo,
                TemplateCode = templateCode,
                ExpirationUtc = expirationUtc,
                Category = category,
                ContentAttributes = contentAttributes
            };
        }

        public static EmailMessage CreateSingleEmailMessageFromTemplate(string emailTo, string subject, string templateCode,
            DateTime? expirationUtc = null, string category = null, Dictionary<string, string> contentAttributes = null)
        {
            return new EmailMessage()
            {
                EmailTo = emailTo,
                Subject = subject,
                TemplateCode = templateCode,
                ExpirationUtc = expirationUtc,
                Category = category,
                ContentAttributes = contentAttributes

            };
        }
    }
}