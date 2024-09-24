using MassTransit;
using MessageHub.Core.Abstraction.Interfaces;
using MessageHub.Domain.Events;
using MessageHub.Services.Consumers.Base;
using MessageHub.Shared;
using Microsoft.Extensions.Logging;

namespace MessageHub.Infrastructure.ServiceBus
{
    public class EmailConsumer(ILogger<EmailConsumer> logger, ISenderService senderService) : BaseSenderConsumer<EmailQueuedEvent>(logger, senderService)
    {
        protected override async Task SendMessageAsync(ConsumeContext<EmailQueuedEvent> context)
        {
            await SenderService.SendMessageAsync(context.Message.MessageId, context.Message.EmailMessage, context.Message.SenderCode, MessageType.EMAIL);
        }
    }
}