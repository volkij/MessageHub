using MassTransit;
using MessageHub.Core.Abstraction.Interfaces;
using MessageHub.Domain.Entities;
using MessageHub.Domain.Enums;
using MessageHub.Domain.Events;
using MessageHub.Domain.Exceptions;
using MessageHub.Infrastructure.Repositories;
using MessageHub.Services.Base;
using MessageHub.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MessageHub.Services
{
    /// <summary>
    /// Service for sending messages to provider
    /// </summary>
    public class SenderService(ILogger<SenderService> logger, IOptions<List<SenderConfig>> senders, UnitOfWork unitOfWork,
        ProviderFactory providerFactory, IPublishEndpoint publishEndpoint) : BaseService(logger, unitOfWork), ISenderService
    {
        private readonly List<SenderConfig> _senders = senders.Value;
        private readonly ProviderFactory _providerFactory = providerFactory;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

        public SenderConfig GetSenderByCode(string senderCode)
        {
            var senderConfig = _senders.FirstOrDefault(s => s.Code.Equals(senderCode, StringComparison.OrdinalIgnoreCase))
                ?? throw new MessageHubException($"Sender with code {senderCode} not found");
            return senderConfig;
        }

        public async Task SendMessageAsync<TMessage>(int messageID, TMessage message, string senderCode, MessageType messageType)
        {
            Message dbMessage = UnitOfWork.MessageRepository.GetById(messageID);
            try
            {
                var senderConfig = this.GetSenderByCode(senderCode);
                var provider = _providerFactory.GetProvider<TMessage>(senderConfig, messageType);


                dbMessage.MessageStatus = MessageStatus.Sent;
                dbMessage.SentDate = DateTime.UtcNow;

                Logger.LogInformation("Sending {messageType} message {messageID}", messageType, messageID);

                var result = provider.SendAsync(message).Result;

                if (!result.IsSuccess)
                {
                    dbMessage.MessageStatus = MessageStatus.SentFailed;
                }

                await _publishEndpoint.Publish(new ProviderResultEvent(result, dbMessage.Id), context =>
                {
                    context.SetRoutingKey(Infrastructure.ServiceBus.RoutingKeys.New);
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error while sending message {messageID}", messageID);
                dbMessage.MessageStatus = MessageStatus.SentFailed;

            }
            finally
            {
                await UnitOfWork.SaveChangesAsync();
            }
        }

    }
}