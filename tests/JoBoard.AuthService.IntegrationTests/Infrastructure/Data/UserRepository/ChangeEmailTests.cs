using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Tests.Common;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.IntegrationTests.Infrastructure.Data.UserRepository;

public class ChangeEmailTests : BaseRepositoryTest
{
    [Fact]
    public async Task RequestEmailChangeAsync()
    {
        // Arrange
        await UnitOfWork.BeginTransactionAsync();
        var user = await UserRepository.FindByIdAsync(DatabaseUserFixtures.ExistingActiveUser.Id);
        var oldEmail = user!.Email;
        var token = ConfirmationTokenFixtures.CreateNew();
        var newEmail = new Email("new-email@gmail.com");
        
        // Act
        user.RequestEmailChange(newEmail, token);
        await UserRepository.UpdateAsync(user);
        await UnitOfWork.CommitAsync();

        // Assert
        var savedUser = await UserRepository.FindByIdAsync(DatabaseUserFixtures.ExistingActiveUser.Id);
        Assert.Equal(oldEmail, savedUser!.Email);
        Assert.Equal(newEmail, savedUser.NewEmail);
        Assert.Equal(token, savedUser.NewEmailConfirmationToken);
    }
    
    [Fact]
    public async Task ChangeEmailAsync()
    {
        // Arrange
        await UnitOfWork.BeginTransactionAsync();
        var user = await UserRepository.FindByIdAsync(DatabaseUserFixtures.ExistingActiveUser.Id);
        var token = ConfirmationTokenFixtures.CreateNew();
        var newEmail = new Email("new-email@gmail.com");
        user!.RequestEmailChange(newEmail, token);
        
        // Act
        user.ChangeEmail(token.Value);
        await UserRepository.UpdateAsync(user);
        await UnitOfWork.CommitAsync();

        // Assert
        var savedUser = await UserRepository.FindByIdAsync(DatabaseUserFixtures.ExistingActiveUser.Id);
        Assert.Equal(newEmail, savedUser!.Email);
        Assert.Null(savedUser!.NewEmail);
        Assert.Null(savedUser!.NewEmailConfirmationToken);
    }
}