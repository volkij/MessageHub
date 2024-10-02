using MessageHub.Domain.Exceptions;
using MessageHub.Shared;
using Microsoft.Extensions.Options;

namespace MessageHub.Services;

/// <summary>
/// Service for managing sender configurations
/// </summary>
public class SenderConfigurationService
{
    private readonly List<SenderConfig> _sendersConfigurationList;

    public SenderConfigurationService(IOptions<List<SenderConfig>> sendersConfigurationList)
    {
        _sendersConfigurationList = sendersConfigurationList.Value;
    }
    
    /// <summary>
    /// Get sender configuration by SenderCode
    /// </summary>
    /// <param name="senderCode"></param>
    /// <returns></returns>
    /// <exception cref="MessageHubException"></exception>
    public SenderConfig GetSenderConfiguration(string senderCode)
    {
        return _sendersConfigurationList
                               .FirstOrDefault(s => s.Code.Equals(senderCode, StringComparison.OrdinalIgnoreCase))
                           ?? throw new MessageHubException($"Sender with code {senderCode} not found");
    }
}