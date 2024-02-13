using System.Net.Http.Json;
using JoBoard.AuthService.Application.UseCases.Auth.Login.ByGoogle;
using JoBoard.AuthService.Tests.Common;

namespace JoBoard.AuthService.FunctionalTests.API.AuthV1Controller;

public class LoginByGoogleTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient; // one per fact
    
    public LoginByGoogleTests(CustomWebApplicationFactory factory) // SetUp
    {
        factory.ResetDatabase();
        _httpClient = factory.CreateClient();
    }
    
    [Fact]
    public async Task Login()
    {
        var request = new LoginByGoogleAccountCommand
        {
            GoogleIdToken = GoogleFixture.IdTokenForExistingUser
        };
        
        var response = await _httpClient.PostAsJsonAsync(AuthV1Routes.LoginByGoogle, request);
        
        await Assert.SuccessAuthResponseAsync(response);
    }
    
    // TODO add test with empty
    // TODO add test with non existing user
    // TODO add test with invalid token
}