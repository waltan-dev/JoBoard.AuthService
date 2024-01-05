using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Domain.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public class ExternalAccount : Entity<UserId>
{
    public string ExternalUserId { get; }
    public ExternalAccountProvider Provider { get; }

    private ExternalAccount() { } // only for ef core 
    
    public ExternalAccount(UserId userId, string externalUserId, ExternalAccountProvider provider)
    {
        Guard.IsNotNullOrWhiteSpace(externalUserId);

        Id = userId;
        ExternalUserId = externalUserId;
        Provider = provider;
    }
}

public enum ExternalAccountProvider
{
    Google = 1
}