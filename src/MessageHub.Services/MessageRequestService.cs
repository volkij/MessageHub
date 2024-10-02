using MessageHub.Core.Abstraction.Interfaces;
using MessageHub.Core.Config;
using MessageHub.Domain.DTO;
using MessageHub.Domain.Entities;
using MessageHub.Domain.Enums;
using MessageHub.Domain.Exceptions;
using MessageHub.Domain.Validators;
using MessageHub.Infrastructure;
using MessageHub.Infrastructure.Repositories;
using MessageHub.Services.Base;
using MessageHub.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MessageHub.Services;

/// <summary>
/// Service for processing incoming message requests
/// </summary>
public class MessageRequestService : BaseService, IMessageRequestService
{
    private readonly AccountConfig _accountConfig;
    private readonly SenderConfigurationService _senderConfigurationService;
    private readonly TemplateService _templateService;
    private readonly MessageService _messageService;
    private readonly IEnumerable<IMessageProcessingService> _messageProcessingServices;
    
    public MessageRequestService(ILogger<MessageRequestService> logger, IOptions<AccountConfig> accountConfig,
        SenderConfigurationService senderConfigurationService, TemplateService templateService,
        MessageService messageService, IEnumerable<IMessageProcessingService> messageProcessingServices) : base(logger)
    {
        _accountConfig = accountConfig.Value;
        _senderConfigurationService = senderConfigurationService;
        _templateService = templateService;
        _messageService = messageService;
        _messageProcessingServices = messageProcessingServices;
    }
    
    public async Task ProcessRequestToMessage(CreateMessageRequest request, string accountName)
        {
            Message messageEntity = Mapper.MapToEntity(request);
            messageEntity.Account = accountName;

            // If sender code is not provided, use the default sender for the account
            if (string.IsNullOrEmpty(messageEntity.SenderCode))
            {
                Account account = _accountConfig.GetAccount(accountName);

                messageEntity.SenderCode = messageEntity.MessageType switch
                {
                    MessageType.EMAIL => account.DefaultSenderEmail,
                    MessageType.SMS => account.DefaultSenderSms,
                    MessageType.PUSH => account.DefaultSenderPush,
                    _ => throw new MessageHubException("Unknown message type")
                };
            }

            var senderConfig = _senderConfigurationService.GetSenderConfiguration(messageEntity.SenderCode);

            messageEntity = await _templateService.ProcessMessage(messageEntity);

            await _messageService.InsertAsync(messageEntity);

            if (messageEntity.MessageStatus != MessageStatus.Failed)
            {
                MessageServiceValidator messageValidator = new MessageServiceValidator();
                var validationResult = messageValidator.Validate(messageEntity);

                if (validationResult.IsValid)
                {
                    if (messageEntity.Expiration < DateTime.UtcNow)
                    {
                        await _messageService.ChangeMessageStatus(messageEntity, MessageStatus.Expired);
                        return;
                    }

                    var processor = _messageProcessingServices.FirstOrDefault(p => p.MessageType == messageEntity.MessageType);
                    if (processor == null)
                    {
                        throw new MessageHubException("Invalid message type");
                    }
                    await processor.ProcessMessage(messageEntity, senderConfig);
                }
                else
                {
                    foreach (var error in validationResult.Errors)
                    {
                        Logger.LogWarning($"Message {messageEntity.Id} validation error: {error.ErrorMessage}");
                    }

                    await _messageService.ChangeMessageStatus(messageEntity, MessageStatus.Failed);
                }
            }

        }
}