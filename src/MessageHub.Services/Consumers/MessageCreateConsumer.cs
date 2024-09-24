using MassTransit;
using MessageHub.Domain.Events;
using MessageHub.Services;
using MessageHub.Services.Consumers.Base;
using Microsoft.Extensions.Logging;

namespace MessageHub.Infrastructure.ServiceBus
{
    public class MessageCreateConsumer(ILogger<MessageCreateConsumer> logger, MessageService messageService) : BaseConsumer(logger), IConsumer<MessageCreateEvent>
    {
        private readonly MessageService _messageService = messageService;

        public async Task Consume(ConsumeContext<MessageCreateEvent> context)
        {
            try
            {
                Logger.LogInformation("Received message: {MessageId}", context.MessageId);

                await _messageService.ProcessRequestToMessage(context.Message.CreateMessageRequest, context.Message.AccountName);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error when consuming a message");
            }

            //return Task.CompletedTask;
        }
    }
}