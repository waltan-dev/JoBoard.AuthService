using JoBoard.AuthService.Tests.Common;

namespace JoBoard.AuthService.Tests.Integration.Infrastructure.Data.UserRepository;

public class AddUserTests : BaseRepositoryTest
{
    [Fact]
    public async Task AddUserRegisteredByEmailAsync()
    {
        // Arrange
        await UserRepository.UnitOfWork.BeginTransactionAsync();
        var newUser = new UserBuilder().Build();

        // Act
        await UserRepository.AddAsync(newUser);
        await UserRepository.UnitOfWork.CommitTransactionAsync();

        // Assert
        var savedUser = await UserRepository.FindByIdAsync(newUser.Id);
        Assert.NotNull(savedUser);
        Assert.Equal(newUser.Id, savedUser!.Id);
        Assert.Equal(newUser.RegisteredAt, savedUser.RegisteredAt);
        Assert.Equal(newUser.Email, savedUser.Email);
        Assert.Equal(newUser.EmailConfirmed, savedUser.EmailConfirmed);
        Assert.Equal(newUser.PasswordHash, savedUser.PasswordHash);
        Assert.Equal(newUser.Role, savedUser.Role);
        Assert.Equal(newUser.Status, savedUser.Status);
        Assert.Equal(newUser.FullName, savedUser.FullName);
        Assert.Equal(newUser.EmailConfirmToken, savedUser.EmailConfirmToken);
        Assert.Equal(newUser.ChangeEmailConfirmToken, savedUser.ChangeEmailConfirmToken);
        Assert.Equal(newUser.ResetPasswordConfirmToken, savedUser.ResetPasswordConfirmToken);
        
        Assert.Equal(newUser.ExternalAccounts, savedUser.ExternalAccounts);
    }
    
    [Fact]
    public async Task AddUserRegisteredByGoogleAsync()
    {
        // Arrange
        await UserRepository.UnitOfWork.BeginTransactionAsync();
        var newUser = new UserBuilder().WithGoogleAccount().Build();

        // Act
        await UserRepository.AddAsync(newUser);
        await UserRepository.UnitOfWork.CommitTransactionAsync();

        // Assert
        var savedUser = await UserRepository.FindByIdAsync(newUser.Id);
        Assert.NotNull(savedUser);
        Assert.Equal(newUser.Id, savedUser!.Id);
        Assert.Equal(newUser.RegisteredAt, savedUser.RegisteredAt);
        Assert.Equal(newUser.Email, savedUser.Email);
        Assert.Equal(newUser.EmailConfirmed, savedUser.EmailConfirmed);
        Assert.Equal(newUser.PasswordHash, savedUser.PasswordHash);
        Assert.Equal(newUser.Role, savedUser.Role);
        Assert.Equal(newUser.Status, savedUser.Status);
        Assert.Equal(newUser.FullName, savedUser.FullName);
        Assert.Equal(newUser.EmailConfirmToken, savedUser.EmailConfirmToken);
        Assert.Equal(newUser.ChangeEmailConfirmToken, savedUser.ChangeEmailConfirmToken);
        Assert.Equal(newUser.ResetPasswordConfirmToken, savedUser.ResetPasswordConfirmToken);
        Assert.Equal(newUser.ExternalAccounts, savedUser.ExternalAccounts);
    }
}