namespace MessageHub.Shared
{
    public interface IProvider<TMessage>
    {
        MessageType MessageType { get; }
        Task<ProviderResult> SendAsync(TMessage message);
    }
}
