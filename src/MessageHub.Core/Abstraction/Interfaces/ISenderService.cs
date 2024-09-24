using MessageHub.Shared;

namespace MessageHub.Core.Abstraction.Interfaces
{
    public interface ISenderService
    {
        SenderConfig GetSenderByCode(string senderCode);

        Task SendMessageAsync<TMessage>(int messageID, TMessage message, string senderCode, MessageType messageType);
    }
}
