using Ardalis.GuardClauses;
using JoBoard.AuthService.Domain.Core;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public class ExternalNetworkAccount : ValueObject
{
    public string ExternalUserId { get; }
    public ExternalNetwork Network { get; }
    
    public ExternalNetworkAccount(string externalUserId, ExternalNetwork network)
    {
        Guard.Against.NullOrWhiteSpace(externalUserId);
        
        ExternalUserId = externalUserId;
        Network = network;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ExternalUserId;
        yield return Network;
    }
}