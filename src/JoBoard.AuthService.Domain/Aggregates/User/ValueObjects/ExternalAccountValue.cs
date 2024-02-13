using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;

public class ExternalAccountValue : ValueObject
{
    public string ExternalUserId { get; private set; }
    public ExternalAccountProvider Provider { get; private set; }

    private ExternalAccountValue() { } // only for ef core 
    
    public ExternalAccountValue(string externalUserId, ExternalAccountProvider provider)
    {
        Guard.IsNotNullOrWhiteSpace(externalUserId);
        
        ExternalUserId = externalUserId;
        Provider = provider;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ExternalUserId;
        yield return Provider;
    }
}

public enum ExternalAccountProvider
{
    Google = 1
}