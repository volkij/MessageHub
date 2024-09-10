using MessageHub.Core.Config;
using MessageHub.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MessageHub.Api.Controllers
{
    /// <summary>
    /// Base controller for all controllers with APIKey
    /// </summary>
    public class BaseAccountController : ControllerBase
    {
        protected readonly ILogger<BaseAccountController> Logger;
        private readonly AccountConfig _accountConfig;

        public BaseAccountController(ILogger<BaseAccountController> logger, IOptions<AccountConfig> accountConfig)
        {
            Logger = logger;
            _accountConfig = accountConfig.Value;
        }

        private Account? _account { get; set; }

        public Account Account
        {
            get
            {
                if (_account == null)
                {
                    if (!Request.Headers.TryGetValue("X-Api-Key", out var extractedApiKey))
                    {
                        throw new MessageHubException("API Key was not provided.");
                    }

                    _account = _accountConfig.Accounts.FirstOrDefault(k => k.ApiKey == extractedApiKey)
                               ?? throw new MessageHubException("API Key is not valid.");
                }
                return _account;
            }
        }
    }
}