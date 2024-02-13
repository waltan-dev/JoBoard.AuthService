using System.Net.Http.Json;
using JoBoard.AuthService.Application.UseCases.Auth.Login.ByGoogle;
using JoBoard.AuthService.Tests.Common;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.FunctionalTests.API.AuthV1Controller;

public class LoginByGoogleTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    
    public LoginByGoogleTests(CustomWebApplicationFactory factory) // Setup for each fact
    {
        _httpClient = factory.CreateClient();
    }
    
    [Fact]
    public async Task Login()
    {
        var request = new LoginByGoogleAccountCommand
        {
            GoogleIdToken = GoogleFixtures.IdTokenForExistingUser
        };
        
        var response = await _httpClient.PostAsJsonAsync(AuthV1Routes.LoginByGoogle, request);
        
        await Assert.SuccessAuthResponseAsync(response);
    }
    
    // TODO add test with empty
    // TODO add test with non existing user
    // TODO add test with invalid token
}