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

namespace MessageHub.Services
{
    /// <summary>
    /// Service for managed messages entities
    /// </summary>
    public class MessageService : BaseRepositoryService
    {
        public MessageService(ILogger<MessageService> logger, UnitOfWork unitOfWork) : base(logger, unitOfWork)
        {
            
        }

        public async Task<Message> InsertAsync(Message message)
        {
            await UnitOfWork.MessageRepository.InsertAsync(message);
            await UnitOfWork.SaveChangesAsync();
            return message;
        }
        
        public async Task<Message> GetByIdAsync(int id)
        {
            return await UnitOfWork.MessageRepository.GetByIdAsync(id);
        }

        public async Task ChangeMessageStatus(Message message, MessageStatus status)
        {
            message.MessageStatus = status;
            await UnitOfWork.SaveChangesAsync();
        }
    
        public async Task MarkMessageAsSent(Message message)
        {
            message.SentDate = DateTime.UtcNow;
            await ChangeMessageStatus(message, MessageStatus.Sent);
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