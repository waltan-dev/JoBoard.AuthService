using System.Net.Http.Json;
using JoBoard.AuthService.Application.UseCases.Auth.ResetPassword.Confirmation;
using JoBoard.AuthService.Application.UseCases.Auth.ResetPassword.Request;
using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.SeedWork;
using JoBoard.AuthService.Tests.Common;
using Microsoft.Extensions.DependencyInjection;

namespace JoBoard.AuthService.FunctionalTests.API.AuthV1Controller;

public class ResetPasswordTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient; // one per fact
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public ResetPasswordTests(CustomWebApplicationFactory factory) // SetUp
    {
        factory.ResetDatabase();
        _httpClient = factory.CreateClient();
        
        var scope = factory.Services.CreateScope();
        _userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        _unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    }
    
    [Fact]
    public async Task RequestPasswordReset()
    {
        var request = new RequestPasswordResetCommand()
        {
            Email = UserFixtures.ExistingActiveUser.Email.Value
        };
        
        var response = await _httpClient.PostAsJsonAsync(AuthV1Routes.RequestPasswordReset, request);

        await Assert.SuccessEmptyResponseAsync(response);
    }
    
    [Fact]
    public async Task RequestPasswordResetWithoutConfirmedEmail()
    {
        var request = new RequestPasswordResetCommand()
        {
            Email = UserFixtures.ExistingUserRegisteredByEmail.Email.Value
        };
        
        var response = await _httpClient.PostAsJsonAsync(AuthV1Routes.RequestPasswordReset, request);

        await Assert.ConflictResponseAsync(response);
    }
    
    // TODO add test with duplicate request
    // TODO add test with empty
    // TODO add test with invalid email
    
    [Fact]
    public async Task ConfirmPasswordReset()
    {
        // Arrange
        await _unitOfWork.BeginTransactionAsync();
        var user = await _userRepository.FindByIdAsync(UserFixtures.ExistingActiveUser.Id);
        var token = UserFixtures.CreateNewConfirmationToken();
        user!.RequestPasswordReset(token);
        await _userRepository.UpdateAsync(user);
        await _unitOfWork.CommitAsync();
        
        // Act
        var request = new ConfirmPasswordResetCommand
        {
            UserId = UserFixtures.ExistingActiveUser.Id.Value,
            ConfirmationToken = token.Value,
            NewPassword = "NewValidPassword123$"
        };
        var response = await _httpClient.PostAsJsonAsync(AuthV1Routes.ConfirmPasswordReset, request);

        // Assert
        await Assert.SuccessEmptyResponseAsync(response);
    }
    
    // TODO add test with non strength password
}