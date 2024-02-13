using System.Net.Http.Json;
using JoBoard.AuthService.Application.UseCases.Auth.Login.CanLoginByGoogle;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.FunctionalTests.API.TokenAuthV1Controller;

public class TokenByGoogleTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    
    public TokenByGoogleTests(CustomWebApplicationFactory factory) // Setup for each fact
    {
        _httpClient = factory.CreateClient();
    }
    
    [Fact]
    public async Task TokenByGoogle()
    {
        var request = new CanLoginByGoogleAccountCommand
        {
            GoogleIdToken = GoogleFixtures.IdTokenForExistingUser
        };
        
        var response = await _httpClient.PostAsJsonAsync(AuthTokenV1Routes.TokenByGoogle, request);
        
        await Assert.SuccessAuthResponseAsync(response);
    }
    
    // TODO add test with empty
    // TODO add test with non existing user
    // TODO add test with invalid token
}