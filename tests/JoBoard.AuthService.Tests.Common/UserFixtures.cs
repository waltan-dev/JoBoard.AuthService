using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.Tests.Common;

public static class UserFixtures
{
    private static User? _existingActiveUser;
    public static User ExistingActiveUser 
    {
        get
        {
            if (_existingActiveUser != null)
                return _existingActiveUser;
            
            var token = ConfirmationToken.Generate();
            var user = new User(
                userId: UserId.Generate(),
                fullName: new FullName("Test", "Hirer"),
                email: new Email("ExistingActiveUser@gmail.com"),
                role: UserRole.Hirer,
                passwordHash:
                "10000.uccK9GykTGkF/hCHyd9KNA==.BbMWJJzz6GlfpcKdmPVJaryNiNiev8kD66fpc2NrzPg=", // passwordHasher.Hash("password"),
                registerConfirmToken: token);
            user.ConfirmEmail(token.Value);
            _existingActiveUser = user;
            
            return _existingActiveUser; 
        }
    }

    public static readonly string DefaultPassword = "ValidPassword123#";
    public static readonly string DefaultPasswordHash = "10000.sV/vif/8IYZO52XF9Tfn5w==.Mi4Hj8hZxlmtkqNlpNRDe1rxrA/Z+6szDZLwQ9vjBrs=";
    
    public static readonly User ExistingUserRegisteredByEmail = new(
        userId: UserId.Generate(),
        fullName: new FullName("Test", "Hirer"),
        email: new Email("ExistingUserRegisteredByEmail@gmail.com"),
        role: UserRole.Hirer, 
        passwordHash: DefaultPasswordHash,
        registerConfirmToken: CreateNewConfirmationToken()); 
    
    public static readonly User ExistingUserRegisteredByExternalAccount = new(
        userId: UserId.Generate(),
        fullName: new FullName("Test", "Worker"),
        email: new Email("ExistingUserRegisteredByExternalAccount@gmail.com"),
        role: UserRole.Worker, 
        externalAccount: new ExternalAccount("1", ExternalAccountProvider.Google)); 
    
    public static readonly User ExistingUserWithExpiredToken = new(
        userId: UserId.Generate(),
        fullName: new FullName("Test", "Hirer"),
        email: new Email("ExistingUserWithExpiredToken@gmail.com"),
        role: UserRole.Hirer, 
        passwordHash: DefaultPasswordHash,
        registerConfirmToken: CreateExpiredConfirmationToken());
    
    public static ConfirmationToken CreateNewConfirmationToken()
    {
        return ConfirmationToken.Generate();
    }
    
    public static ConfirmationToken CreateExpiredConfirmationToken()
    {
        return ConfirmationToken.Generate(-1);
    }
}