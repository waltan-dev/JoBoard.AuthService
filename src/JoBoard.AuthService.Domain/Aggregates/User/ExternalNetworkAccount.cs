using CommunityToolkit.Diagnostics;
using JoBoard.AuthService.Domain.SeedWork;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public class ExternalNetworkAccount : Entity<UserId>
{
    public string ExternalUserId { get; }
    public ExternalNetwork Network { get; }

    private ExternalNetworkAccount() { }
    
    public ExternalNetworkAccount(UserId userId, string externalUserId, ExternalNetwork network)
    {
        Guard.IsNotNullOrWhiteSpace(externalUserId);

        Id = userId;
        ExternalUserId = externalUserId;
        Network = network;
    }
}