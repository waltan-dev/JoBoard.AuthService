using System.Net.Http.Json;
using JoBoard.AuthService.Application.Commands.ResetPassword.Confirmation;
using JoBoard.AuthService.Tests.Functional.Fixtures;
using Assert = JoBoard.AuthService.Tests.Functional.Extensions.Assert;

namespace JoBoard.AuthService.Tests.Functional.Tests.API.AccountV1Controller;

public class ConfirmPasswordResetTests : BaseApiTest
{
    public ConfirmPasswordResetTests(
        CustomWebApplicationFactory webApplicationFactory,
        DatabaseFixture databaseFixture,
        RedisFixture redisFixture) : base(webApplicationFactory, databaseFixture, redisFixture) { }
    
    [Fact]
    public async Task CheckSuccess()
    {
        // Arrange
        await UserRepository.UnitOfWork.BeginTransactionAsync();
        var user = await UserRepository.FindByIdAsync(DbUserFixtures.ExistingActiveUser01.Id);
        var token = FunctionalTestsRegistry.ConfirmationTokenBuilder.BuildActive();
        user!.RequestPasswordReset(token, FunctionalTestsRegistry.CurrentDateTime);
        await UserRepository.UpdateAsync(user);
        await UserRepository.UnitOfWork.CommitTransactionAsync();
        
        var request = new ConfirmPasswordResetCommand
        {
            UserId = DbUserFixtures.ExistingActiveUser01.Id.Value,
            ConfirmationToken = token.Value,
            NewPassword = "NewValidPassword123$"
        };
        
        // Act
        var response = await HttpClient.PostAsJsonAsync(AccountV1Routes.ConfirmPasswordReset, request);

        // Assert
        await Assert.SuccessEmptyResponseAsync(response);
    }
    
    [Fact]
    public async Task CheckNotFound()
    {
        // Arrange
        var request = new ConfirmPasswordResetCommand
        {
            UserId = Guid.NewGuid(),
            ConfirmationToken = "some-token",
            NewPassword = "NewValidPassword123$"
        };
        
        // Act
        var response = await HttpClient.PostAsJsonAsync(AccountV1Routes.ConfirmPasswordReset, request);

        // Assert
        await Assert.NotFoundResponseAsync(response);
    }

    [Fact]
    public async Task CheckConflict()
    {
        // Arrange
        var request = new ConfirmPasswordResetCommand
        {
            UserId = DbUserFixtures.ExistingActiveUser01.Id.Value,
            ConfirmationToken = "invalid-token",
            NewPassword = "NewValidPassword123$"
        };
        
        // Act
        var response = await HttpClient.PostAsJsonAsync(AccountV1Routes.ConfirmPasswordReset, request);

        // Assert
        await Assert.ConflictResponseAsync(response);
    }
    
    [Fact]
    public async Task CheckValidation()
    {
        // Arrange
        var request = new ConfirmPasswordResetCommand
        {
            UserId = Guid.Empty,
            ConfirmationToken = " ",
            NewPassword = "non-strength-password"
        };
        
        // Act
        var response = await HttpClient.PostAsJsonAsync(AccountV1Routes.ConfirmPasswordReset, request);

        // Assert
        await Assert.ValidationResponseAsync(response, 3);
    }
}