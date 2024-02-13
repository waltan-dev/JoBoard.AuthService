using System.Net.Http.Json;
using JoBoard.AuthService.Application.Commands.Account.Login.CanLoginByPassword;
using JoBoard.AuthService.Tests.Common.DataFixtures;


namespace JoBoard.AuthService.Tests.Functional.API.TokenAuthV1Controller;

public class TokenByPasswordTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    
    public TokenByPasswordTests(CustomWebApplicationFactory factory) // Setup for each fact
    {
        _httpClient = factory.CreateClient();
    }
    
    [Fact]
    public async Task TokenByPassword()
    {
        var request = new CanLoginByPasswordCommand()
        {
            Email = DbUserFixtures.ExistingUserWithoutConfirmedEmail.Email.Value,
            Password = PasswordFixtures.DefaultPassword,
        };
        
        var response = await _httpClient.PostAsJsonAsync(AuthTokenV1Routes.TokenByPassword, request);
        
        await Assert.SuccessAuthResponseAsync(response);
    }
    
    // TODO add test with empty
    // TODO add test with invalid email
    // TODO add test with invalid password
    // TODO add test with non-existing email
}