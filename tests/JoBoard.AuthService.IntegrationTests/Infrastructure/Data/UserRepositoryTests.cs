using JoBoard.AuthService.Infrastructure.Data.Repositories;
using JoBoard.AuthService.Tests.Common;

namespace JoBoard.AuthService.IntegrationTests.Infrastructure.Data;

public class UserRepositoryTests : BaseRepositoryTest
{
    private readonly UserRepository _userRepository;
    
    public UserRepositoryTests()
    {
        _userRepository = new UserRepository(_dbContext);
    }
    
    [Fact]
    public async Task AddUserAsync()
    {
        await _unitOfWork.StartTransactionAsync();
        var newUser = new UserBuilder().Build();

        await _userRepository.AddAsync(newUser);
        await _unitOfWork.CommitAsync();

        var savedUser = await _userRepository.FindByIdAsync(newUser.Id);
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
}