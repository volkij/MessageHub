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
    /// Service for sending messages to providers
    /// </summary>
    public class SenderService : BaseService, ISenderService
    {

        private readonly SenderConfigurationService _senderConfigurationService;
        private readonly IProviderFactory _providerFactory;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly MessageService _messageService;
        
        /// <param name="sendersConfigurationList">List of senders configurations</param>
        /// <param name="logger"></param>
        /// <param name="unitOfWork"></param>
        /// <param name="providerFactory">Provider factory class for managing providers instances</param>
        /// <param name="publishEndpoint"></param>
        public SenderService(SenderConfigurationService senderConfigurationService, ILogger<SenderService> logger, MessageService messageService,
            IProviderFactory providerFactory, IPublishEndpoint publishEndpoint) : base(logger)
        {
            _senderConfigurationService = senderConfigurationService;
            _messageService = messageService;
            _providerFactory = providerFactory;
            _publishEndpoint = publishEndpoint;
        }
        
        /// <summary>
        /// Send message to specific provider.
        /// </summary>
        /// <param name="messageID"></param>
        /// <param name="message"></param>
        /// <param name="senderCode"></param>
        /// <param name="messageType"></param>
        /// <typeparam name="TMessage"></typeparam>
        public async Task SendMessageAsync<TMessage>(int messageID, TMessage message, string senderCode, MessageType messageType)
        {
            var dbMessage = await _messageService.GetByIdAsync(messageID);
            try
            {
                var senderConfig = _senderConfigurationService.GetSenderConfiguration(senderCode);
                var provider = _providerFactory.GetProvider<TMessage>(senderConfig, messageType);

                await _messageService.MarkMessageAsSent(dbMessage);
              
                Logger.LogInformation("Sending {messageType} message {messageID}", messageType, messageID);

                var result = await provider.SendAsync(message);

                if (!result.IsSuccess)
                {
                    await _messageService.ChangeMessageStatus(dbMessage, MessageStatus.SentFailed);
                }

                await _publishEndpoint.Publish(new ProviderResultEvent(result, dbMessage.Id), context =>
                {
                    context.SetRoutingKey(Infrastructure.ServiceBus.RoutingKeys.New);
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error while sending message {messageID}", messageID);
                await _messageService.ChangeMessageStatus(dbMessage, MessageStatus.SentFailed);
            }
        }
    }
}