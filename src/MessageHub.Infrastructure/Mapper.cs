using MessageHub.Domain.Enums;
using MessageHub.Shared;

namespace MessageHub.Infrastructure
{
    public static class Mapper
    {
        public static Domain.Entities.Message MapToEntity(Domain.DTO.CreateMessageRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            if (!Enum.TryParse<MessageType>(request.Type.ToUpper(), true, out var messageType))
            {
                throw new ArgumentException($"Invalid message type: {request.Type}");
            }

            var entity = new Domain.Entities.Message
            {
                MessageType = messageType,
                ContactValue = request.ContactValue,
                CreateDate = DateTime.UtcNow,
                Content = request.Content,
                Subject = request.Subject,
                TemplateCode = request.TemplateCode,
                CampaignCode = request.CampaignCode,
                ExternalMessageID = request.ExternalMessageID,
                Expiration = request.Expiration,
                Priority = request.Priority ?? 1,
                Category = request.Category,
                Tags = request.Tags,
                MessageStatus = MessageStatus.Created,
                ServiceName = request.ServiceName,
                SenderCode = request.SenderCode,
                ExternalClientID = request.ExternalClientID,
                Attachments = request.Attachments?.Select(a => new Domain.Entities.MessageAttachment
                {
                    FileName = a.FileName,
                    FileContent = a.FileContent
                }).ToList(),
                ContentAttributes = request.ContentAttributes?.Select(ca => new Domain.Entities.MessageAttribute
                {
                    Key = ca.Name,
                    Value = ca.Value
                }).ToList()
            };

            return entity;
        }
    }
}
