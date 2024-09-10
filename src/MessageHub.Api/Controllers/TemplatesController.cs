using Asp.Versioning;
using MessageHub.Core.Config;
using MessageHub.Domain.DTO;
using MessageHub.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MessageHub.Api.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class TemplatesController(ILogger<MessagesController> logger, IOptions<AccountConfig> accountConfig, TemplateDownloadService templateDownService, TemplateService templateService) : BaseAccountController(logger, accountConfig)
    {
        private readonly TemplateDownloadService _templateDownService = templateDownService;
        private readonly TemplateService _templateService = templateService;

        [MapToApiVersion(1)]
        [HttpGet]
        public async Task<IActionResult> GetTemplates()
        {
            var result = await _templateService.GetAllTemplates();

            return Ok(result);
        }

        [MapToApiVersion(1)]
        [HttpPost("reload")]
        public async Task<IActionResult> ReloadTemplates([FromBody] TemplateReloadRequest request)
        {
            if (string.IsNullOrEmpty(request.Url))
            {
                return BadRequest("Url is required.");
            }

            var templatesCount = await _templateDownService.DownloadTemplatesAsync(request.Url);
            return Ok(templatesCount);
        }
    }
}
