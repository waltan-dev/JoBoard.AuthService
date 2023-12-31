using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Domain.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public class ExternalNetworkAccount : ValueObject
{
    public string ExternalUserId { get; }
    public ExternalNetwork Network { get; }
    
    public ExternalNetworkAccount(string externalUserId, ExternalNetwork network)
    {
        Guard.IsNotNullOrWhiteSpace(externalUserId);
        
        ExternalUserId = externalUserId;
        Network = network;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ExternalUserId;
        yield return Network;
    }
}