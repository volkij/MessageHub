using MessageHub.Domain.DTO;

namespace MessageHub.Core.Abstraction.Interfaces;

public interface IMessageRequestService
{
    Task ProcessRequestToMessage(CreateMessageRequest request, string accountName);
}