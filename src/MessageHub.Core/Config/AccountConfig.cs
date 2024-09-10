using MessageHub.Domain.Exceptions;

namespace MessageHub.Core.Config
{
    /// <summary>
    /// Configuration for application accounts
    /// </summary>
    public class AccountConfig
    {
        public required List<Account> Accounts { get; set; } = new List<Account>();

        public Account GetAccount(string accountName)
        {
            var account = Accounts.FirstOrDefault(p => p.Name == accountName) ??
                throw new MessageHubException($"Account {accountName} not found");
            return account;
        }
    }

    public class Account
    {
        public string Name { get; set; }
        public string ApiKey { get; set; }
        public string DefaultSenderEmail { get; set; }
        public string DefaultSenderSms { get; set; }
        public string DefaultSenderPush { get; set; }
    }
}