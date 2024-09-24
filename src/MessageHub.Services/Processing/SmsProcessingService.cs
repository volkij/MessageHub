using MassTransit;
using MessageHub.Core.Abstraction.Interfaces;
using MessageHub.Domain.Entities;
using MessageHub.Domain.Events;
using MessageHub.Infrastructure.Repositories;
using MessageHub.Shared;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace MessageHub.Services.Processing
{
    public class SmsProcessingService(ILogger<MessageService> logger, UnitOfWork unitOfWork, IPublishEndpoint publishEndpoint) : BaseProcessingService(logger, unitOfWork, publishEndpoint), IMessageProcessingService
    {
        public MessageType MessageType => MessageType.SMS;

        public async Task ProcessMessage(Message message, SenderConfig senderConfig)
        {
            string phone = message.ContactValue.Replace(" ", "");
            string text = RemoveSpecialCharacters(message.Content);

            SmsMessage smsMessage = new(senderConfig.PhoneName, phone, text);

            await QueueMessage(new SmsQueuedEvent(smsMessage, message.Id, senderConfig.Code), message);
        }

        private string RemoveSpecialCharacters(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            string normalizedString = input.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            string result = stringBuilder.ToString().Normalize(NormalizationForm.FormC);
            string expression = @"[^a-zA-Z0-9\s\.\,\-\!\?\+""'\(\)\[\]]";
            result = Regex.Replace(result, expression, string.Empty);

            return result;
        }
    }
}