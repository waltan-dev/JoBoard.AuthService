using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.UnitTests.Domain.Aggregates.User;

public static class UserTestsHelper
{
    public static readonly UserId DefaultUserId = UserId.Generate();
    public static readonly FullName DefaultFullName = new("Ivan", "Ivanov");
    public static readonly Email DefaultEmail = new("ivan@gmail.com");
    public static readonly UserRole DefaultUserRole = UserRole.Hirer;
    public static readonly string DefaultPasswordHash = "DefaultPasswordHash";
    public static readonly string DefaultPassword = "DefaultPassword";
    public static readonly ConfirmationToken DefaultConfirmationToken = 
        new(Guid.NewGuid().ToString(), DateTime.UtcNow.AddHours(TokenExpiresInHours));
    public static readonly ExternalAccount DefaultExternalAccount =
        new("externalUserId", ExternalAccountProvider.Google);

    public const int TokenExpiresInHours = 24;

    public static ConfirmationToken CreateNewConfirmationToken()
    {
        return new ConfirmationToken("new-token", DateTime.UtcNow.AddHours(TokenExpiresInHours));
    }
}