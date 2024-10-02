using MessageHub.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace MessageHub.Services.Base;

/// <summary>
/// Base class for repository/entity services
/// </summary>
public abstract class BaseRepositoryService(ILogger<BaseRepositoryService> logger, UnitOfWork unitOfWork) : BaseService(logger)
{
    protected readonly UnitOfWork UnitOfWork = unitOfWork;
}