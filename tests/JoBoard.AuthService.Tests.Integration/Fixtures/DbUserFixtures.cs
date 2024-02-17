using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.Contracts;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Tests.Common.Fixtures;
using Moq;

namespace JoBoard.AuthService.Tests.Integration.Fixtures;

// Thread-safe Singleton for parallel tests
public static class DbUserFixtures
{
    public static readonly User ExistingActiveUser = BuildExistingActiveUser();
    
    public static IEnumerable<User> List => new List<User>()
    {
        ExistingActiveUser
    };
    
    private static User BuildExistingActiveUser()
    {
        return IntegrationTestsRegistry.UserBuilder
            .WithEmail("ExistingActiveUser@gmail.com")
            .WithActiveStatus()
            .Build();
    }
}