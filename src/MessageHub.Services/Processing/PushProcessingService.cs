using MassTransit;
using MessageHub.Core.Abstraction.Interfaces;
using MessageHub.Domain.Entities;
using MessageHub.Domain.Events;
using MessageHub.Infrastructure.Repositories;
using MessageHub.Shared;
using Microsoft.Extensions.Logging;

namespace MessageHub.Services.Processing
{
    public class PushProcessingService(ILogger<MessageService> logger, UnitOfWork unitOfWork, IPublishEndpoint publishEndpoint) : BaseProcessingService(logger, unitOfWork, publishEndpoint), IMessageProcessingService
    {
        public MessageType MessageType => MessageType.PUSH;

        public void ProcessMessage(Message message, SenderConfig senderConfig)
        {
            List<PushToken> listOfToken = UnitOfWork.PushTokenRepository.GetValidByExternalClient(message.ExternalClientID, message.Account, message.SenderCode);

            if (listOfToken.Count == 0)
                throw new Exception("No tokens for PushMessage");

            PushMessage pushMessage = new(message.Subject, message.Content, listOfToken.Select(x => x.Value).ToList(), message.ExternalMessageID);

            QueueMessage(new PushQueuedEvent(pushMessage, message.Id, senderConfig.Code), message);
        }
    }
}