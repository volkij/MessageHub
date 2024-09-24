using MassTransit;
using MessageHub.Domain.Entities;
using MessageHub.Domain.Enums;
using MessageHub.Infrastructure.Repositories;
using MessageHub.Services.Base;
using Microsoft.Extensions.Logging;


namespace MessageHub.Services.Processing
{
    /// <summary>
    /// Base class for modification message and publish to ServiceBus
    /// </summary>
    public class BaseProcessingService(ILogger<BaseService> logger, UnitOfWork unitOfWork, IPublishEndpoint publishEndpoint) : BaseService(logger, unitOfWork)
    {
        protected readonly IPublishEndpoint PublishEndpoint = publishEndpoint;

        protected async Task QueueMessage<T>(T eventMessage, Message message) where T : class
        {
            message.MessageStatus = MessageStatus.Queued;
            await UnitOfWork.SaveChangesAsync();

            await PublishEndpoint.Publish(eventMessage, context =>
            {
                context.SetRoutingKey(Infrastructure.ServiceBus.RoutingKeys.New);
            });
        }
    }
}
