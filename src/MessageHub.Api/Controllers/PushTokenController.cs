using Asp.Versioning;
using MessageHub.Core.Config;
using MessageHub.Domain.DTO;
using MessageHub.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;

namespace MessageHub.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class PushTokenController(ILogger<PushTokenController> logger, IOptions<AccountConfig> accountConfig, PushTokenService pushTokenService) : BaseAccountController(logger, accountConfig)
    {
        private readonly PushTokenService _pushTokenService = pushTokenService;

        [HttpPost]
        public async Task<IActionResult> CreatePushToken([FromBody] CreatePushTokenRequest request)
        {
            Logger.LogDebug("New request: CreatePushToken");

            bool created = await _pushTokenService.SavePushToken(this.Account, request);

            if (created) return StatusCode((int)HttpStatusCode.Created);
            else return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}