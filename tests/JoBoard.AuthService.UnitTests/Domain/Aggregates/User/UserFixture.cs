using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.UnitTests.Domain.Aggregates.User;

public static class UserFixture
{
    public static readonly UserId DefaultUserId = UserId.Generate();
    public static readonly FullName DefaultFullName = new("Ivan", "Ivanov");
    public static readonly Email DefaultEmail = new("ivan@gmail.com");
    public static readonly UserRole DefaultUserRole = UserRole.Hirer;
    public static readonly string DefaultPasswordHash = "DefaultPasswordHash";
    public static readonly string DefaultPassword = "DefaultPassword";
    public static readonly ConfirmationToken DefaultConfirmationToken = ConfirmationToken.Generate();
    public static readonly ExternalAccount DefaultGoogleAccount =
        new("googleUserId", ExternalAccountProvider.Google);
    
    public static ConfirmationToken CreateNewConfirmationToken()
    {
        return ConfirmationToken.Generate();
    }
    
    public static ConfirmationToken CreateExpiredConfirmationToken()
    {
        return ConfirmationToken.Generate(-1);
    }
}