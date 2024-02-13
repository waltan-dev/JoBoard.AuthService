using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Infrastructure.Data.Repositories;
using JoBoard.AuthService.Tests.Common;

namespace JoBoard.AuthService.IntegrationTests.Infrastructure.Data;

public class UserRepositoryTests : BaseRepositoryTest
{
    [Fact]
    public async Task AddAsync()
    {
        // Arrange
        await UnitOfWork.StartTransactionAsync();
        var newUser = new UserBuilder().Build();

        // Act
        await UserRepository.AddAsync(newUser);
        await UnitOfWork.CommitAsync();

        // Assert
        var savedUser = await UserRepository.FindByIdAsync(newUser.Id);
        Assert.NotNull(savedUser);
        Assert.Equal(newUser.Id, savedUser!.Id);
        Assert.Equal(newUser.RegisteredAt, savedUser.RegisteredAt);
        Assert.Equal(newUser.Email, savedUser.Email);
        Assert.Equal(newUser.EmailConfirmed, savedUser.EmailConfirmed);
        Assert.Equal(newUser.ExternalAccounts, savedUser.ExternalAccounts);
        Assert.Equal(newUser.PasswordHash, savedUser.PasswordHash);
        Assert.Equal(newUser.Role, savedUser.Role);
        Assert.Equal(newUser.Status, savedUser.Status);
        Assert.Equal(newUser.FullName, savedUser.FullName);
        Assert.Equal(newUser.RegisterConfirmToken, savedUser.RegisterConfirmToken);
        Assert.Equal(newUser.NewEmailConfirmationToken, savedUser.NewEmailConfirmationToken);
        Assert.Equal(newUser.ResetPasswordConfirmToken, savedUser.ResetPasswordConfirmToken);
    }
    
    [Fact]
    public async Task RequestEmailChangeAsync()
    {
        // Arrange
        await UnitOfWork.StartTransactionAsync();
        var user = await UserRepository.FindByIdAsync(UserFixtures.ExistingActiveUser.Id);
        var oldEmail = user!.Email;
        var token = UserFixtures.CreateNewConfirmationToken();
        var newEmail = new Email("new-email@gmail.com");
        
        // Act
        user.RequestEmailChange(newEmail, token);
        await UserRepository.UpdateAsync(user);
        await UnitOfWork.CommitAsync();

        // Assert
        var savedUser = await UserRepository.FindByIdAsync(UserFixtures.ExistingActiveUser.Id);
        Assert.Equal(oldEmail, savedUser!.Email);
        Assert.Equal(newEmail, savedUser.NewEmail);
        Assert.Equal(token, savedUser.NewEmailConfirmationToken);
    }
    
    [Fact]
    public async Task ChangeEmailAsync()
    {
        // Arrange
        await UnitOfWork.StartTransactionAsync();
        var user = await UserRepository.FindByIdAsync(UserFixtures.ExistingActiveUser.Id);
        var token = UserFixtures.CreateNewConfirmationToken();
        var newEmail = new Email("new-email@gmail.com");
        user!.RequestEmailChange(newEmail, token);
        
        // Act
        user.ChangeEmail(token.Value);
        await UserRepository.UpdateAsync(user);
        await UnitOfWork.CommitAsync();

        // Assert
        var savedUser = await UserRepository.FindByIdAsync(UserFixtures.ExistingActiveUser.Id);
        Assert.Equal(newEmail, savedUser!.Email);
        Assert.Null(savedUser!.NewEmail);
        Assert.Null(savedUser!.NewEmailConfirmationToken);
    }
}