using MessageHub.Shared;
using Microsoft.Extensions.Logging;
using System.Reflection;

/// <summary>
/// Create or retrieve provider instances
/// </summary>
public class ProviderFactory
{
    private readonly ILogger<ProviderFactory> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<(string, MessageType), object> _providers = new Dictionary<(string, MessageType), object>();

    public ProviderFactory(ILogger<ProviderFactory> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public IProvider<TMessage> GetProvider<TMessage>(SenderConfig senderConfig, MessageType messageType)
    {
        var key = (senderConfig.Code, messageType);

        if (!_providers.ContainsKey(key))
        {
            Type type = GetType(senderConfig.ClassName, senderConfig.AssemblyName);

            _logger.LogDebug($"Creating instance of provider for {senderConfig.Code} and message type {messageType}");

            var service = Activator.CreateInstance(type, _logger, senderConfig) as IProvider<TMessage>;
            if (service == null || service.MessageType != messageType)
            {
                throw new InvalidOperationException("Service not found or does not implement IProvider for the correct message type");
            }

            _providers[key] = service;
        }

        return (IProvider<TMessage>)_providers[key];
    }

    private static Type GetType(string className, string assemblyName)
    {
        Assembly assembly = Assembly.Load(assemblyName);
        if (assembly == null)
        {
            throw new ArgumentException($"Assembly {assemblyName} not found.");
        }
        Type type = assembly.GetType(className);
        return type;
    }
}