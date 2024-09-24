using MassTransit;
using MessageHub.Core.Abstraction.Interfaces;
using MessageHub.Domain.Events;
using MessageHub.Services.Consumers.Base;
using MessageHub.Shared;
using Microsoft.Extensions.Logging;

namespace MessageHub.Services.Consumers
{
    public class PushConsumer(ILogger<PushConsumer> logger, ISenderService senderService) : BaseSenderConsumer<PushQueuedEvent>(logger, senderService), IConsumer<PushQueuedEvent>
    {
        protected override async Task SendMessageAsync(ConsumeContext<PushQueuedEvent> context)
        {
            await SenderService.SendMessageAsync(context.Message.MessageId, context.Message.PushMessage, context.Message.SenderCode, MessageType.PUSH);
        }
    }
}
