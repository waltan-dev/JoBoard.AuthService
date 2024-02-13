using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Domain.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public class ExternalAccount : ValueObject
{
    public UserId Id { get; init; }
    public string ExternalUserId { get; init; }
    public ExternalAccountProvider Provider { get; init; }

    private ExternalAccount() { } // only for ef core 
    
    public ExternalAccount(string externalUserId, ExternalAccountProvider provider)
    {
        Guard.IsNotNullOrWhiteSpace(externalUserId);
        
        ExternalUserId = externalUserId;
        Provider = provider;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
        yield return ExternalUserId;
        yield return Provider;
    }
}

public enum ExternalAccountProvider
{
    Google = 1
}