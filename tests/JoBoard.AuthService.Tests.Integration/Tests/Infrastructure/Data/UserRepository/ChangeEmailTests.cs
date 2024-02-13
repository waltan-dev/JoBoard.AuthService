using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Tests.Integration.Fixtures;

namespace JoBoard.AuthService.Tests.Integration.Tests.Infrastructure.Data.UserRepository;

public class ChangeEmailTests : BaseRepositoryTest
{
    public ChangeEmailTests(DatabaseFixture databaseFixture) : base(databaseFixture)
    {
    }
    
    [Fact]
    public async Task RequestEmailChangeAsync()
    {
        // Arrange
        await UserRepository.UnitOfWork.BeginTransactionAsync();
        var user = await UserRepository.FindByIdAsync(DbUserFixtures.ExistingActiveUser.Id);
        var oldEmail = user!.Email;
        var token = IntegrationTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var newEmail = new Email("new-email@gmail.com");
        var userEmailUniquenessChecker = IntegrationTestsRegistry.UserEmailUniquenessChecker;
        
        // Act
        user.RequestEmailChange(newEmail, token, userEmailUniquenessChecker, IntegrationTestsRegistry.CurrentDateTime);
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
        var token = IntegrationTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        var newEmail = new Email("new-email@gmail.com");
        var userEmailUniquenessChecker = IntegrationTestsRegistry.UserEmailUniquenessChecker;
        user!.RequestEmailChange(newEmail, token, userEmailUniquenessChecker, IntegrationTestsRegistry.CurrentDateTime);
        
        // Act
        user.ConfirmEmailChange(token.Value, IntegrationTestsRegistry.CurrentDateTime);
        await UserRepository.UpdateAsync(user);
        await UserRepository.UnitOfWork.CommitTransactionAsync();

        // Assert
        var savedUser = await UserRepository.FindByIdAsync(DbUserFixtures.ExistingActiveUser.Id);
        Assert.Equal(newEmail, savedUser!.Email);
        Assert.Null(savedUser.NewEmail);
        Assert.Null(savedUser.ChangeEmailConfirmToken);
    }
}