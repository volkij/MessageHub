using MessageHub.Domain.DTO;
using MessageHub.Domain.Entities;
using MessageHub.Domain.Enums;
using MessageHub.Infrastructure.Repositories;
using MessageHub.Services.Base;
using Microsoft.Extensions.Logging;

namespace MessageHub.Services
{
    /// <summary>
    /// Service for managing templates
    /// </summary>
    public class TemplateService(ILogger<TemplateService> logger, UnitOfWork unitOfWork) : BaseRepositoryService(logger, unitOfWork)
    {
        /// <summary>
        /// Apply template to message
        /// </summary>
        public async Task<Message> ProcessMessage(Message message)
        {
            if (!string.IsNullOrEmpty(message.TemplateCode))
            {
                Template? template = await UnitOfWork.TemplateRepository.GetTemplateByCodeAsync(message.TemplateCode);
                if (template == null)
                {
                    message.MessageStatus = MessageStatus.Failed;
                    return message;
                }

                message.Content = template.Content;
                if (!string.IsNullOrEmpty(template.Subject))
                {
                    message.Subject = template.Subject;
                }
            }

            if (!string.IsNullOrEmpty(message.Content))
            {
                message.Content = ReplaceContentAttributes(message.Content, message);
            }

            if (!string.IsNullOrEmpty(message.Subject))
            {
                message.Subject = ReplaceContentAttributes(message.Subject, message);
            }

            return message;
        }

        public async Task<List<GeTemplateResponse>> GetAllTemplates()
        {
            var templates = await UnitOfWork.TemplateRepository.GetAllTemplates();
            return templates.Select(t => new GeTemplateResponse
            {
                ID = t.Id,
                Type = t.Type,
                Name = t.Name,
                Code = t.Code,
                Url = t.Url,
                Subject = t.Subject,
                DateMod = t.DateMod
            }).ToList();
        }

        /// <summary>
        /// Replace variables in content with attributes from message
        /// </summary>
        private string ReplaceContentAttributes(string content, Message message)
        {
            if (message.ContentAttributes == null) return content;

            foreach (var attribute in message.ContentAttributes)
            {
                string keyPattern1 = $"{{{{{attribute.Key}}}}}";
                string keyPattern2 = $"$@{attribute.Key}@$";
                content = content.Replace(keyPattern1, attribute.Value)
                                 .Replace(keyPattern2, attribute.Value);
            }

            return content;
        }
    }
}