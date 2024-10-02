using MassTransit;
using MessageHub.Core.Abstraction.Interfaces;
using MessageHub.Domain.Entities;
using MessageHub.Domain.Events;
using MessageHub.Infrastructure.Repositories;
using MessageHub.Shared;
using Microsoft.Extensions.Logging;

namespace MessageHub.Services.Processing
{
    public class PushProcessingService(ILogger<PushProcessingService> logger, UnitOfWork unitOfWork, IPublishEndpoint publishEndpoint) : BaseProcessingService(logger, unitOfWork, publishEndpoint), IMessageProcessingService
    {
        public MessageType MessageType => MessageType.PUSH;

        public async Task ProcessMessage(Message message, SenderConfig senderConfig)
        {
            List<PushToken> listOfToken = await UnitOfWork.PushTokenRepository.GetValidByExternalClientAsync(message.ExternalClientID, message.Account, message.SenderCode);

            if (listOfToken.Count == 0)
                throw new Exception("No tokens for PushMessage");

            PushMessage pushMessage = new(message.Subject, message.Content, listOfToken.Select(x => x.Value).ToList(), message.ExternalMessageID);

            await QueueMessage(new PushQueuedEvent(pushMessage, message.Id, senderConfig.Code), message);
        }
    }
}