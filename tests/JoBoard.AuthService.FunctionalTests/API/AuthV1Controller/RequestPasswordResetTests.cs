using System.Net.Http.Json;
using JoBoard.AuthService.Application.UseCases.Auth.ResetPassword.Request;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.FunctionalTests.API.AuthV1Controller;

public class RequestPasswordResetTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    
    public RequestPasswordResetTests(CustomWebApplicationFactory factory) // Setup for each fact
    {
        _httpClient = factory.CreateClient();
    }
    
    [Fact]
    public async Task RequestPasswordReset()
    {
        var request = new RequestPasswordResetCommand()
        {
            Email = DatabaseUserFixtures.ExistingActiveUser.Email.Value
        };
        
        var response = await _httpClient.PostAsJsonAsync(AuthV1Routes.RequestPasswordReset, request);

        await Assert.SuccessEmptyResponseAsync(response);
    }
    
    [Fact]
    public async Task RequestPasswordResetWithoutConfirmedEmail()
    {
        var request = new RequestPasswordResetCommand()
        {
            Email = DatabaseUserFixtures.ExistingUserRegisteredByEmail.Value.Email.Value
        };
        
        var response = await _httpClient.PostAsJsonAsync(AuthV1Routes.RequestPasswordReset, request);

        await Assert.ConflictResponseAsync(response);
    }
    
    // TODO add test with duplicate request
    // TODO add test with empty
    // TODO add test with invalid email
}