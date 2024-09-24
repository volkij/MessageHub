using MassTransit;
using MessageHub.Domain.Entities;
using MessageHub.Domain.Enums;
using MessageHub.Domain.Events;
using MessageHub.Services.Consumers.Base;
using Microsoft.Extensions.Logging;

namespace MessageHub.Services.Consumers
{
    public class ProviderResultConsumer(ILogger<ProviderResultConsumer> logger, MessageService messageService, PushTokenService pushTokenService) : BaseConsumer(logger), IConsumer<ProviderResultEvent>
    {
        private readonly MessageService _messageService = messageService;
        private readonly PushTokenService _pushTokenService = pushTokenService;

        public async Task Consume(ConsumeContext<ProviderResultEvent> context)
        {
            try
            {
                Logger.LogInformation($"Received MessageResult: {context.MessageId}");

                Message message = await _messageService.GetByIdAsync(context.Message.MessageId);
                await _messageService.ChangeMessageStatus(message, MessageStatus.Finished);

                if (message.MessageType == Shared.MessageType.PUSH)
                {
                    foreach (var tokenValue in context.Message.ProviderResult.UndeliveryContacts)
                    {
                        await _pushTokenService.ExpirePushToken(message.Account, tokenValue);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error when consuming a MessageResult");
            }
        }
    }
}