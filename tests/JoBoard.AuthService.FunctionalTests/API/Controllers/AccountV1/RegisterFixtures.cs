using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.SeedWork;
using JoBoard.AuthService.Infrastructure.Data;

namespace JoBoard.AuthService.FunctionalTests.API.Controllers.AccountV1;

public static class RegisterFixtures
{
    public static readonly User ExistingUserRegisteredByEmail = new(
        userId: UserId.Generate(),
        fullName: new FullName("Test", "Hirer"),
        email: new Email("ExistingUserRegisteredByEmail@gmail.com"),
        role: UserRole.Hirer, 
        passwordHash: "10000.uccK9GykTGkF/hCHyd9KNA==.BbMWJJzz6GlfpcKdmPVJaryNiNiev8kD66fpc2NrzPg=", // passwordHasher.Hash("password"),
        registerConfirmToken: ConfirmationToken.Generate()); 
    
    public static readonly User ExistingUserRegisteredByExternalAccount = new(
        userId: UserId.Generate(),
        fullName: new FullName("Test", "Worker"),
        email: new Email("ExistingUserRegisteredByExternalAccount@gmail.com"),
        role: UserRole.Worker, 
        externalAccount: new ExternalAccount("1", ExternalAccountProvider.Google),
        registerConfirmToken: ConfirmationToken.Generate()); 
    
    public static readonly User ExistingUserWithExpiredToken = new(
        userId: UserId.Generate(),
        fullName: new FullName("Test", "Hirer"),
        email: new Email("ExistingUserWithExpiredToken@gmail.com"),
        role: UserRole.Hirer, 
        passwordHash: "password-hash",
        registerConfirmToken: ConfirmationToken.Generate(-1));
}