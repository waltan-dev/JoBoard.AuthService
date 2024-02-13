using System.Net.Http.Json;
using JoBoard.AuthService.Application.UseCases.Auth.Register.ByGoogle;
using JoBoard.AuthService.Tests.Common;

namespace JoBoard.AuthService.FunctionalTests.API.AuthV1Controller;

public class RegisterByGoogleTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient; // one per fact
    
    public RegisterByGoogleTests(CustomWebApplicationFactory factory) // SetUp
    {
        factory.ResetDatabase();
        _httpClient = factory.CreateClient();
    }
    
    [Fact]
    public async Task RegisterByGoogle()
    {
        var request = new RegisterByGoogleAccountCommand
        {
            GoogleIdToken = GoogleFixture.IdTokenForNewUser,
            Role = "Hirer"
        };
        
        var response = await _httpClient.PostAsJsonAsync(AuthV1Routes.RegisterByGoogle, request);
        
        await Assert.SuccessAuthResponseAsync(response);
    }
    
    // TODO add test with empty
    // TODO add test with invalid token
}