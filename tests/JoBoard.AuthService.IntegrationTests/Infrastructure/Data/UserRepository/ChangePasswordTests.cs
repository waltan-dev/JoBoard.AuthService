using JoBoard.AuthService.Tests.Common;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.IntegrationTests.Infrastructure.Data.UserRepository;

public class ChangePasswordTests : BaseRepositoryTest
{
    [Fact]
    public async Task RequestPasswordResetAsync()
    {
        // Arrange
        await UnitOfWork.BeginTransactionAsync();
        var user = await UserRepository.FindByIdAsync(DatabaseUserFixtures.ExistingActiveUser.Id);
        var token = ConfirmationTokenFixtures.CreateNew();
        
        // Act
        user!.RequestPasswordReset(token);
        await UserRepository.UpdateAsync(user);
        await UnitOfWork.CommitAsync();

        // Assert
        var savedUser = await UserRepository.FindByIdAsync(DatabaseUserFixtures.ExistingActiveUser.Id);
        Assert.Equal(token, savedUser!.ResetPasswordConfirmToken);
    }
}