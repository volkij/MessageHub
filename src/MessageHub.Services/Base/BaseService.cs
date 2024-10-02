using MessageHub.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace MessageHub.Services.Base
{
    /// <summary>
    /// Base class for services with logger
    /// </summary>
    /// <param name="logger"></param>
    public abstract class BaseService(ILogger<BaseService> logger)
    {
        protected readonly ILogger<BaseService> Logger = logger;
    }
}
