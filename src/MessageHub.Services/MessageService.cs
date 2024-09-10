using MessageHub.Core.Abstraction.Interfaces;
using MessageHub.Core.Config;
using MessageHub.Domain.DTO;
using MessageHub.Domain.Entities;
using MessageHub.Domain.Enums;
using MessageHub.Domain.Exceptions;
using MessageHub.Domain.Validators;
using MessageHub.Infrastructure;
using MessageHub.Infrastructure.Repositories;
using MessageHub.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MessageHub.Services
{
    public class MessageService : BaseService
    {
        private readonly ISenderService _senderService;
        private readonly TemplateService _templateService;
        private readonly IEnumerable<IMessageProcessingService> _messageProcessingServices;
        private readonly AccountConfig _accountConfig;

        public MessageService(ILogger<MessageService> logger, UnitOfWork unitOfWork, IOptions<AccountConfig> accountConfig, ISenderService senderService,
            TemplateService templateService, IEnumerable<IMessageProcessingService> messageProcessingServices) : base(logger, unitOfWork)
        {
            _senderService = senderService;
            _templateService = templateService;
            _messageProcessingServices = messageProcessingServices;
            _accountConfig = accountConfig.Value;
        }

        public void ProcessRequestToMessage(CreateMessageRequest request, string accountName)
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

            SenderConfig senderConfig = _senderService.GetSenderByCode(messageEntity.SenderCode);


            messageEntity = _templateService.ProcessMessage(messageEntity);

            UnitOfWork.MessageRepository.Insert(messageEntity);
            UnitOfWork.SaveChanges();

            MessageServiceValidator messageValidator = new MessageServiceValidator();
            var validationResult = messageValidator.Validate(messageEntity);

            if (validationResult.IsValid)
            {
                if (messageEntity.Expiration < DateTime.UtcNow)
                {
                    ChangeMessageStatus(messageEntity, MessageStatus.Expired);
                    return;
                }

                var processor = _messageProcessingServices.FirstOrDefault(p => p.MessageType == messageEntity.MessageType);
                if (processor == null)
                {
                    throw new MessageHubException("Invalid message type");
                }
                processor.ProcessMessage(messageEntity, senderConfig);
            }
            else
            {
                foreach (var error in validationResult.Errors)
                {
                    Logger.LogWarning($"Message {messageEntity.Id} validation error: {error.ErrorMessage}");
                }

                ChangeMessageStatus(messageEntity, MessageStatus.Failed);
            }
        }

        public Message GetById(int id)
        {
            return UnitOfWork.MessageRepository.GetById(id);
        }

        public void ChangeMessageStatus(Message message, MessageStatus status)
        {
            message.MessageStatus = status;
            UnitOfWork.SaveChanges();
        }

        public async Task<List<GetMessageResponse>> GetMessagesByTypeAndExternalClientAsync(string messageType, string externalClientID)
        {
            MessageType type = Enum.Parse<MessageType>(messageType, true);

            List<Message> messages = await UnitOfWork.MessageRepository.GetMessagesByTypeAndExternalClientAsync(type, externalClientID);

            List<GetMessageResponse> responseList = messages.Select(r => new GetMessageResponse
            {
                ID = r.Id,
                Type = r.MessageType.ToString(),
                ExternalClientID = r.ExternalClientID,
                Subject = r.Subject,
                Content = r.Content,
                Status = r.MessageStatus.ToString(),
                Category = r.Category,
                SentDate = r.SentDate,
                ReadDate = r.ReadDate
            }).ToList();

            return responseList;
        }


        public async Task MarkMessageAsRead(int messageId)
        {
            var message = await UnitOfWork.MessageRepository.GetByIdAsync(messageId)
                             ?? throw new NotFoundException("Message not found");

            if (message.ReadDate == null)
            {
                message.ReadDate = DateTime.UtcNow;
                await UnitOfWork.SaveChangesAsync();
            }
        }
    }
}