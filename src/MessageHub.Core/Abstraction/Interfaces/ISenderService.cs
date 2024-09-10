using MessageHub.Shared;

namespace MessageHub.Core.Abstraction.Interfaces
{
    public interface ISenderService
    {
        SenderConfig GetSenderByCode(string senderCode);

        void SendMessage<TMessage>(int messageID, TMessage message, string senderCode, MessageType messageType);
    }
}
