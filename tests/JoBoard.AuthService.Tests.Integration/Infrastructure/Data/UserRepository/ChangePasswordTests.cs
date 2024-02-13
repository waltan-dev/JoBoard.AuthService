using JoBoard.AuthService.Tests.Common.Builders;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.Tests.Integration.Infrastructure.Data.UserRepository;

public class ChangePasswordTests : BaseRepositoryTest
{
    [Fact]
    public async Task RequestPasswordResetAsync()
    {
        // Arrange
        await UserRepository.UnitOfWork.BeginTransactionAsync();
        var user = await UserRepository.FindByIdAsync(DbUserFixtures.ExistingActiveUser.Id);
        var token = new ConfirmationTokenBuilder().BuildActive();
        
        // Act
        user!.RequestPasswordReset(token);
        await UserRepository.UpdateAsync(user);
        await UserRepository.UnitOfWork.CommitTransactionAsync();

        // Assert
        var savedUser = await UserRepository.FindByIdAsync(DbUserFixtures.ExistingActiveUser.Id);
        Assert.Equal(token, savedUser!.ResetPasswordConfirmToken);
    }
}