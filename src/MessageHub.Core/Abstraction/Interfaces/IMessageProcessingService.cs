using MessageHub.Domain.Entities;
using MessageHub.Shared;

namespace MessageHub.Core.Abstraction.Interfaces
{
    public interface IMessageProcessingService
    {
        Task ProcessMessage(Message message, SenderConfig senderConfig);
        MessageType MessageType { get; }
    }
}