using System.Net.Http.Json;
using JoBoard.AuthService.Application.UseCases.Account.ResetPassword.Confirmation;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Tests.Common.Fixtures;
using Microsoft.Extensions.DependencyInjection;

namespace JoBoard.AuthService.FunctionalTests.API.AccountV1Controller;

public class ConfirmPasswordResetTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public ConfirmPasswordResetTests(CustomWebApplicationFactory factory) // Setup for each fact
    {
        _httpClient = factory.CreateClient();
        
        var scope = factory.Services.CreateScope();
        _userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    }
    
    [Fact]
    public async Task ConfirmPasswordReset()
    {
        // Arrange
        await _unitOfWork.BeginTransactionAsync();
        var user = await _userRepository.FindByIdAsync(DatabaseUserFixtures.ExistingActiveUser.Id);
        var token = ConfirmationTokenFixtures.CreateNew();
        user!.RequestPasswordReset(token);
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.CommitAsync();
        
        // Act
        var request = new ConfirmPasswordResetCommand
        {
            UserId = DatabaseUserFixtures.ExistingActiveUser.Id.Value,
            ConfirmationToken = token.Value,
            NewPassword = "NewValidPassword123$"
        };
        var response = await _httpClient.PostAsJsonAsync(AccountV1Routes.ConfirmPasswordReset, request);

        // Assert
        await Assert.SuccessEmptyResponseAsync(response);
    }
    
    // TODO add test with non strength password
}