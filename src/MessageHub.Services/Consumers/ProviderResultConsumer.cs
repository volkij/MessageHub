using MassTransit;
using MessageHub.Domain.Entities;
using MessageHub.Domain.Enums;
using MessageHub.Domain.Events;
using Microsoft.Extensions.Logging;

namespace MessageHub.Services.Consumers
{
    public class ProviderResultConsumer(ILogger<ProviderResultConsumer> logger, MessageService messageService, PushTokenService pushTokenService) : BaseConsumer(logger), IConsumer<ProviderResultEvent>
    {
        private readonly MessageService _messageService = messageService;
        private readonly PushTokenService _pushTokenService = pushTokenService;

        public Task Consume(ConsumeContext<ProviderResultEvent> context)
        {
            try
            {
                Logger.LogInformation($"Received MessageResult: {context.MessageId}");

                Message message = _messageService.GetById(context.Message.MessageId);
                _messageService.ChangeMessageStatus(message, MessageStatus.Finished);

                if (message.MessageType == Shared.MessageType.PUSH)
                {
                    foreach (var tokenValue in context.Message.ProviderResult.UndeliveryContacts)
                    {
                        _pushTokenService.ExpirePushToken(message.Account, tokenValue);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error when consuming a MessageResult");
            }

            return Task.CompletedTask;
        }
    }
}