using JoBoard.AuthService.Tests.Integration.Fixtures;

namespace JoBoard.AuthService.Tests.Integration.Tests.Infrastructure.Data.UserRepository;

public class AddUserTests : BaseRepositoryTest
{
    public AddUserTests(DatabaseFixture databaseFixture) : base(databaseFixture)
    {
    }

    private const string EmailForUser1 = "test_001@gmail.com";
    private const string EmailForUser2 = "test_002@gmail.com";

    [Fact]
    public async Task AddUserRegisteredByEmailAsync()
    {
        // Arrange
        var newUser = IntegrationTestsRegistry.UserBuilder
            .WithEmail(EmailForUser1)
            .Build();

        // Act
        await UserRepository.UnitOfWork.BeginTransactionAsync();
        await UserRepository.AddAsync(newUser);
        await UserRepository.UnitOfWork.CommitTransactionAsync();

        // Assert
        var savedUser = await UserRepository.FindByIdAsync(newUser.Id);
        Assert.NotNull(savedUser);
        Assert.Equal(newUser.Id, savedUser!.Id);
        Assert.Equal(newUser.RegisteredAt, savedUser.RegisteredAt);
        Assert.Equal(newUser.Email, savedUser.Email);
        Assert.Equal(newUser.EmailConfirmed, savedUser.EmailConfirmed);
        Assert.Equal(newUser.Password, savedUser.Password);
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
        var newUser = IntegrationTestsRegistry.UserBuilder
            .WithEmail(EmailForUser2)
            .WithGoogleAccount("test")
            .Build();

        // Act
        await UserRepository.UnitOfWork.BeginTransactionAsync();
        await UserRepository.AddAsync(newUser);
        await UserRepository.UnitOfWork.CommitTransactionAsync();

        // Assert
        var savedUser = await UserRepository.FindByIdAsync(newUser.Id);
        Assert.NotNull(savedUser);
        Assert.Equal(newUser.Id, savedUser!.Id);
        Assert.Equal(newUser.RegisteredAt, savedUser.RegisteredAt);
        Assert.Equal(newUser.Email, savedUser.Email);
        Assert.Equal(newUser.EmailConfirmed, savedUser.EmailConfirmed);
        Assert.Equal(newUser.Password, savedUser.Password);
        Assert.Equal(newUser.Role, savedUser.Role);
        Assert.Equal(newUser.Status, savedUser.Status);
        Assert.Equal(newUser.FullName, savedUser.FullName);
        Assert.Equal(newUser.EmailConfirmToken, savedUser.EmailConfirmToken);
        Assert.Equal(newUser.ChangeEmailConfirmToken, savedUser.ChangeEmailConfirmToken);
        Assert.Equal(newUser.ResetPasswordConfirmToken, savedUser.ResetPasswordConfirmToken);
        Assert.Equal(newUser.ExternalAccounts, savedUser.ExternalAccounts);
    }
}