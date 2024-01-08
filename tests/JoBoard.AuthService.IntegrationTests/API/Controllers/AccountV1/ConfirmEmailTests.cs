using System.Net.Http.Json;
using JoBoard.AuthService.Application.Auth.ConfirmEmail;

namespace JoBoard.AuthService.IntegrationTests.API.Controllers.AccountV1;

public class ConfirmEmailTests : BaseApiTest, IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient; // one per fact
    
    public ConfirmEmailTests(CustomWebApplicationFactory factory) // SetUp
    {
        _httpClient = factory.CreateClient();
    }
    
    [Fact]
    public async Task ConfirmEmail()
    {
        var request = new ConfirmEmailCommand
        {
            UserId = RegisterFixtures.ExistingUserRegisteredByEmail.Id.Value,
            Token = RegisterFixtures.ExistingUserRegisteredByEmail.RegisterConfirmToken!.Value,
        };
        
        var response = await _httpClient.PostAsJsonAsync("api/v1/account/confirm-email", request);

        await AssertSuccessEmptyResponseAsync(response);
    }
    
    [Fact]
    public async Task ConfirmEmailAfterExpiration()
    {
        var request = new ConfirmEmailCommand
        {
            UserId = RegisterFixtures.ExistingUserWithExpiredToken.Id.Value,
            Token = RegisterFixtures.ExistingUserWithExpiredToken.RegisterConfirmToken!.Value,
        };
        
        var response = await _httpClient.PostAsJsonAsync("api/v1/account/confirm-email", request);

        await AssertConflictResponseAsync(response);
    }
    
    [Fact]
    public async Task ConfirmEmailWithInvalidToken()
    {
        var request = new ConfirmEmailCommand
        {
            UserId = RegisterFixtures.ExistingUserWithExpiredToken.Id.Value,
            Token = "invalid-token",
        };
        
        var response = await _httpClient.PostAsJsonAsync("api/v1/account/confirm-email", request);
        
        await AssertConflictResponseAsync(response);
    }
    
    [Fact]
    public async Task ConfirmEmailWithEmpty()
    {
        var request = new ConfirmEmailCommand
        {
            UserId = Guid.Empty,
            Token = string.Empty
        };
        
        var response = await _httpClient.PostAsJsonAsync("api/v1/account/confirm-email", request);
        
        await AssertValidationResponseAsync(response);
    }
}