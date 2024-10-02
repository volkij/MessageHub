using MessageHub.Shared;

namespace MessageHub.Core.Abstraction.Interfaces
{
    public interface ISenderService
    {
        Task SendMessageAsync<TMessage>(int messageID, TMessage message, string senderCode, MessageType messageType);
    }
}
