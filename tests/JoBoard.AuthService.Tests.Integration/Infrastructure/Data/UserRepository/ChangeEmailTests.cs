using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Tests.Common.Builders;
using JoBoard.AuthService.Tests.Common.DataFixtures;


namespace JoBoard.AuthService.Tests.Integration.Infrastructure.Data.UserRepository;

public class ChangeEmailTests : BaseRepositoryTest
{
    [Fact]
    public async Task RequestEmailChangeAsync()
    {
        // Arrange
        await UserRepository.UnitOfWork.BeginTransactionAsync();
        var user = await UserRepository.FindByIdAsync(DbUserFixtures.ExistingActiveUser.Id);
        var oldEmail = user!.Email;
        var token = new ConfirmationTokenBuilder().BuildActive();
        var newEmail = new Email("new-email@gmail.com");
        
        // Act
        user.RequestEmailChange(newEmail, token);
        await UserRepository.UpdateAsync(user);
        await UserRepository.UnitOfWork.CommitTransactionAsync();

        // Assert
        var savedUser = await UserRepository.FindByIdAsync(DbUserFixtures.ExistingActiveUser.Id);
        Assert.Equal(oldEmail, savedUser!.Email);
        Assert.Equal(newEmail, savedUser.NewEmail);
        Assert.Equal(token, savedUser.ChangeEmailConfirmToken);
    }
    
    [Fact]
    public async Task ChangeEmailAsync()
    {
        // Arrange
        await UserRepository.UnitOfWork.BeginTransactionAsync();
        var user = await UserRepository.FindByIdAsync(DbUserFixtures.ExistingActiveUser.Id);
        var token = new ConfirmationTokenBuilder().BuildActive();
        var newEmail = new Email("new-email@gmail.com");
        user!.RequestEmailChange(newEmail, token);
        
        // Act
        user.ConfirmEmailChange(token.Value);
        await UserRepository.UpdateAsync(user);
        await UserRepository.UnitOfWork.CommitTransactionAsync();

        // Assert
        var savedUser = await UserRepository.FindByIdAsync(DbUserFixtures.ExistingActiveUser.Id);
        Assert.Equal(newEmail, savedUser!.Email);
        Assert.Null(savedUser.NewEmail);
        Assert.Null(savedUser.ChangeEmailConfirmToken);
    }
}