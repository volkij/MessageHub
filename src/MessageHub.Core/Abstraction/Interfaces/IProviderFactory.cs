using MessageHub.Shared;

namespace MessageHub.Core.Abstraction.Interfaces;

/// <summary>
/// Interface for creating or retrieving Sender provider instances
/// </summary>
public interface IProviderFactory
{
    /// <summary>
    /// Get provider instance for a specific sender and message type
    /// </summary>
    /// <param name="senderConfig"></param>
    /// <param name="messageType"></param>
    /// <typeparam name="TMessage"></typeparam>
    /// <returns></returns>
    IProvider<TMessage> GetProvider<TMessage>(SenderConfig senderConfig, MessageType messageType);
    
}