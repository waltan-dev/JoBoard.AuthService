using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;

public class ExternalAccountValue : ValueObject
{
    public string ExternalUserId { get; private set; }
    public ExternalAccountProvider Provider { get; private set; }

    private ExternalAccountValue() { } // only for ef core 
    
    public ExternalAccountValue(string externalUserId, ExternalAccountProvider provider)
    {
        DomainGuard.IsNotNullOrWhiteSpace(externalUserId);
        
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