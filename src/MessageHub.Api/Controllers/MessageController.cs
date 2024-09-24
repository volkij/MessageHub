using Asp.Versioning;
using MessageHub.Core.Config;
using MessageHub.Domain.DTO;
using MessageHub.Domain.Events;
using MessageHub.Infrastructure.ServiceBus;
using MessageHub.Services;
using MessageHub.Services.Telemetry;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MessageHub.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class MessageController(ILogger<MessageController> logger, IOptions<AccountConfig> accountConfig,
        IPublishEndpoint publishEndpoint, MessageHubMetrics messageHubMetrics, MessageService messageService) : BaseAccountController(logger, accountConfig)
    {
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly MessageHubMetrics _messageHubMetrics = messageHubMetrics;
        private readonly MessageService _messageService = messageService;

        [HttpPost]
        public async Task<IActionResult> CreateMessage([FromBody] CreateMessageRequest request)
        {
            Logger.LogDebug("New request: CreateMessage");

            _messageHubMetrics.MessageRequestCreate();
            _messageHubMetrics.MessageByType(request.Type.ToUpper());
            _messageHubMetrics.RecordMessageRequest();

            await _publishEndpoint.Publish(new MessageCreateEvent(request, this.Account.Name), context =>
            {
                context.SetRoutingKey(RoutingKeys.New);
            });

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(string messageType, string externalClientID)
        {
            var result = await _messageService.GetMessagesByTypeAndExternalClientAsync(messageType, externalClientID);
            return Ok(result);
        }

        [HttpPut("{messageID}/markasread")]
        public async Task<IActionResult> MarkAsRead(int messageID)
        {
            await messageService.MarkMessageAsRead(messageID);
            return Ok();
        }
    }
}