using System.Net.Http.Json;
using JoBoard.AuthService.Application.UseCases.Auth.Login.ByEmail;
using JoBoard.AuthService.Tests.Common;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.FunctionalTests.API.AuthV1Controller;

public class LoginByEmailTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    
    public LoginByEmailTests(CustomWebApplicationFactory factory) // Setup for each fact
    {
        _httpClient = factory.CreateClient();
    }
    
    [Fact]
    public async Task Login()
    {
        var request = new LoginByEmailCommand()
        {
            Email = DatabaseUserFixtures.ExistingUserRegisteredByEmail.Value.Email.Value,
            Password = PasswordFixtures.DefaultPassword,
        };
        
        var response = await _httpClient.PostAsJsonAsync(AuthV1Routes.Login, request);
        
        await Assert.SuccessAuthResponseAsync(response);
    }
    
    // TODO add test with empty
    // TODO add test with invalid email
    // TODO add test with invalid password
    // TODO add test with non-existing email
}