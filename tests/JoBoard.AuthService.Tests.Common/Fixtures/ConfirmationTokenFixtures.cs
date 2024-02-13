using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.Tests.Common.Fixtures;

public static class ConfirmationTokenFixtures
{
    public static ConfirmationToken CreateNew()
    {
        return ConfirmationToken.Generate();
    }
    
    public static ConfirmationToken CreateExpired()
    {
        return ConfirmationToken.Generate(-1);
    }
}