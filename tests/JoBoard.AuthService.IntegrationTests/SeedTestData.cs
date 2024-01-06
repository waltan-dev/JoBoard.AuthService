using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Services;
using JoBoard.AuthService.Infrastructure.Data;

namespace JoBoard.AuthService.IntegrationTests;

public static class SeedTestData
{
    public static readonly User User1Hirer = new(
        userId: UserId.Generate(),
        fullName: new FullName("Test", "Hirer"),
        email: new Email("testHirer@gmail.com"),
        role: UserRole.Hirer, 
        passwordHash: "10000.uccK9GykTGkF/hCHyd9KNA==.BbMWJJzz6GlfpcKdmPVJaryNiNiev8kD66fpc2NrzPg=", // passwordHasher.Hash("password"),
        registerConfirmToken: new ConfirmationToken(Guid.NewGuid().ToString(), DateTime.UtcNow.AddHours(24))); 
    
    public static readonly User User2Worker = new(
        userId: UserId.Generate(),
        fullName: new FullName("Test", "Worker"),
        email: new Email("testWorker@gmail.com"),
        role: UserRole.Worker, 
        externalAccount: new ExternalAccount("1", ExternalAccountProvider.Google),
        registerConfirmToken: new ConfirmationToken(Guid.NewGuid().ToString(), DateTime.UtcNow.AddHours(24))); 
    
    public static void Reinitialize(AuthDbContext dbContext)
    {
        var users = dbContext.Users.ToArray();
        dbContext.Users.RemoveRange(users);
        
        dbContext.Users.AddRange(User1Hirer, User2Worker);
        dbContext.SaveChanges();
    }
}