using System.Net.Http.Json;
using JoBoard.AuthService.Application.UseCases.Account.ResetPassword.Confirmation;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Tests.Common.Builders;
using JoBoard.AuthService.Tests.Common.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace JoBoard.AuthService.Tests.Functional.API.AccountV1Controller;

public class ConfirmPasswordResetTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    private readonly IUserRepository _userRepository;
    
    public ConfirmPasswordResetTests(CustomWebApplicationFactory factory) // Setup for each fact
    {
        _httpClient = factory.CreateClient();
        
        var scope = factory.Services.CreateScope();
        _userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
    }
    
    [Fact]
    public async Task ConfirmPasswordReset()
    {
        // Arrange
        await _userRepository.UnitOfWork.BeginTransactionAsync();
        var user = await _userRepository.FindByIdAsync(DbUserFixtures.ExistingActiveUser.Id);
        var token = new ConfirmationTokenBuilder().BuildActive();
        user!.RequestPasswordReset(token);
        await _userRepository.UpdateAsync(user);
        await _userRepository.UnitOfWork.CommitTransactionAsync();
        
        // Act
        var request = new ConfirmPasswordResetCommand
        {
            UserId = DbUserFixtures.ExistingActiveUser.Id.Value,
            ConfirmationToken = token.Value,
            NewPassword = "NewValidPassword123$"
        };
        var response = await _httpClient.PostAsJsonAsync(AccountV1Routes.ConfirmPasswordReset, request);

        // Assert
        await Assert.SuccessEmptyResponseAsync(response);
    }
    
    // TODO add test with non strength password
}