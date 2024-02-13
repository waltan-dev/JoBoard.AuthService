using System.Net.Http.Json;
using JoBoard.AuthService.Application.UseCases.Account.Register.ByGoogle;
using JoBoard.AuthService.Tests.Common.DataFixtures;


namespace JoBoard.AuthService.Tests.Functional.API.AccountV1Controller;

public class RegisterByGoogleTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    
    public RegisterByGoogleTests(CustomWebApplicationFactory factory) // Setup for each fact
    {
        _httpClient = factory.CreateClient();
    }
    
    [Fact]
    public async Task RegisterByGoogle()
    {
        var request = new RegisterByGoogleAccountCommand
        {
            GoogleIdToken = GoogleFixtures.IdTokenForNewUser,
            Role = "Hirer"
        };
        
        var response = await _httpClient.PostAsJsonAsync(AccountV1Routes.RegisterByGoogle, request);
        
        await Assert.SuccessEmptyResponseAsync(response);
    }
    
    // TODO add test with empty
    // TODO add test with invalid token
}