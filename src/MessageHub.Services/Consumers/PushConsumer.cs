using MassTransit;
using MessageHub.Core.Abstraction.Interfaces;
using MessageHub.Domain.Events;
using MessageHub.Shared;
using Microsoft.Extensions.Logging;

namespace MessageHub.Services.Consumers
{
    public class PushConsumer(ILogger<PushConsumer> logger, ISenderService senderService) : BaseSenderConsumer<PushQueuedEvent>(logger, senderService), IConsumer<PushQueuedEvent>
    {
        protected override void SendMessage(ConsumeContext<PushQueuedEvent> context)
        {
            SenderService.SendMessage(context.Message.MessageId, context.Message.PushMessage, context.Message.SenderCode, MessageType.PUSH);
        }
    }
}
