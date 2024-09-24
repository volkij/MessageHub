using MessageHub.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace MessageHub.Services.Base
{
    /// <summary>
    /// Base class for repository services
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="unitOfWork"></param>
    public abstract class BaseService(ILogger<BaseService> logger, UnitOfWork unitOfWork)
    {
        protected readonly UnitOfWork UnitOfWork = unitOfWork;
        protected readonly ILogger<BaseService> Logger = logger;
    }
}
