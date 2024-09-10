using Microsoft.Extensions.Logging;

namespace MessageHub.Services.Consumers
{
    public abstract class BaseConsumer(ILogger<BaseConsumer> logger)
    {
        protected readonly ILogger<BaseConsumer> Logger = logger;
    }
}