using MessageHub.Domain.Entities;
using MessageHub.Shared;

namespace MessageHub.Core.Abstraction.Interfaces
{
    public interface IMessageProcessingService
    {
        void ProcessMessage(Message message, SenderConfig senderConfig);
        MessageType MessageType { get; }
    }
}