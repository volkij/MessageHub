using MessageHub.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace MessageHub.Services
{

    public abstract class BaseService(ILogger<BaseService> logger, UnitOfWork unitOfWork)
    {
        protected readonly UnitOfWork UnitOfWork = unitOfWork;
        protected readonly ILogger<BaseService> Logger = logger;
    }
}
