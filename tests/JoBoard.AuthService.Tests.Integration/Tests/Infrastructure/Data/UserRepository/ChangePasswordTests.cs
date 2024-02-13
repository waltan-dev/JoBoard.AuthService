using JoBoard.AuthService.Tests.Integration.Fixtures;

namespace JoBoard.AuthService.Tests.Integration.Tests.Infrastructure.Data.UserRepository;

public class ChangePasswordTests : BaseRepositoryTest
{
    public ChangePasswordTests(DatabaseFixture databaseFixture) : base(databaseFixture)
    {
    }
    
    [Fact]
    public async Task RequestPasswordResetAsync()
    {
        // Arrange
        await UserRepository.UnitOfWork.BeginTransactionAsync();
        var user = await UserRepository.FindByIdAsync(DbUserFixtures.ExistingActiveUser.Id);
        var token = IntegrationTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        
        // Act
        user!.RequestPasswordReset(token, IntegrationTestsRegistry.CurrentDateTime);
        await UserRepository.UpdateAsync(user);
        await UserRepository.UnitOfWork.CommitTransactionAsync();

        // Assert
        var savedUser = await UserRepository.FindByIdAsync(DbUserFixtures.ExistingActiveUser.Id);
        Assert.Equal(token, savedUser!.ResetPasswordConfirmToken);
    }
}