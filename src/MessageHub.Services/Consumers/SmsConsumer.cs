using MassTransit;
using MessageHub.Core.Abstraction.Interfaces;
using MessageHub.Domain.Events;
using MessageHub.Services.Consumers.Base;
using MessageHub.Shared;
using Microsoft.Extensions.Logging;

namespace MessageHub.Services.Consumers
{
    public class SmsConsumer(ILogger<SmsConsumer> logger, ISenderService senderService) : BaseSenderConsumer<SmsQueuedEvent>(logger, senderService), IConsumer<SmsQueuedEvent>
    {
        protected override async Task SendMessageAsync(ConsumeContext<SmsQueuedEvent> context)
        {
            await SenderService.SendMessageAsync(context.Message.MessageId, context.Message.SmsMessage, context.Message.SenderCode, MessageType.SMS);
        }
    }
}