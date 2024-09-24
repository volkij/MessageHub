using MessageHub.Core.Config;
using MessageHub.Domain.DTO;
using MessageHub.Domain.Entities;
using MessageHub.Infrastructure.Repositories;
using MessageHub.Services.Base;
using Microsoft.Extensions.Logging;

namespace MessageHub.Services
{
    /// <summary>
    /// Service for managing push tokens
    /// </summary>
    public class PushTokenService(ILogger<PushTokenService> logger, UnitOfWork unitOfWork) : BaseService(logger, unitOfWork)
    {
        public async Task<bool> SavePushToken(Account account, CreatePushTokenRequest createPushTokenRequest)
        {
            PushToken pushToken = await UnitOfWork.PushTokenRepository.GetByValueAsync(account.Name, createPushTokenRequest.Value);
            if (pushToken != null) return false;

            pushToken = new PushToken
            {
                Account = account.Name,
                ExternalClientID = createPushTokenRequest.ExternalClientID,
                Value = createPushTokenRequest.Value,
                CreateDate = DateTime.UtcNow
            };

            UnitOfWork.PushTokenRepository.Insert(pushToken);
            await UnitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task ExpirePushToken(string accountCode, string value)
        {
            PushToken pushToken = await UnitOfWork.PushTokenRepository.GetByValueAsync(accountCode, value);
            if (pushToken != null)
            {
                pushToken.ExpirationDate = DateTime.UtcNow;
                await UnitOfWork.SaveChangesAsync();
            }
        }
    }
}
