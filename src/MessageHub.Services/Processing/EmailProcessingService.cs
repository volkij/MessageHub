using MassTransit;
using MessageHub.Core.Abstraction.Interfaces;
using MessageHub.Domain.Entities;
using MessageHub.Domain.Events;
using MessageHub.Infrastructure.Repositories;
using MessageHub.Shared;
using Microsoft.Extensions.Logging;

namespace MessageHub.Services.Processing
{
    public class EmailProcessingService(ILogger<MessageService> logger, UnitOfWork unitOfWork, IPublishEndpoint publishEndpoint) : BaseProcessingService(logger, unitOfWork, publishEndpoint), IMessageProcessingService
    {
        public MessageType MessageType => MessageType.EMAIL;

        public void ProcessMessage(Message message, SenderConfig senderConfig)
        {
            EmailAddress from = new(senderConfig.Email, senderConfig.EmailName);
            EmailAddress to = new(message.ContactValue, string.Empty);
            EmailMessage emailMessage = new(from, to, message.Subject, message.Content, message.Content);


            QueueMessage(new EmailQueuedEvent(emailMessage, message.Id, senderConfig.Code), message);
        }
    }
}