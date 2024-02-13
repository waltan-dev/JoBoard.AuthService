using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.UserAggregate;

public class ExternalAccount : Entity
{
    public new UserId Id { get; private set; }
    public ExternalAccountValue Value { get; private set; }
    
    private ExternalAccount() { } // only for ef core 
    
    public ExternalAccount(UserId userId, ExternalAccountValue value)
    {
        Id = userId;
        Value = value;
    }
}