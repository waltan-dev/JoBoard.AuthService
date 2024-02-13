using System.Net.Http.Json;
using JoBoard.AuthService.Application.Commands.Account.ConfirmEmail;
using JoBoard.AuthService.Tests.Common.DataFixtures;


namespace JoBoard.AuthService.Tests.Functional.API.AccountV1Controller;

public class ConfirmEmailTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    
    public ConfirmEmailTests(CustomWebApplicationFactory factory) // Setup for each fact
    {
        _httpClient = factory.CreateClient();
    }
    
    [Fact]
    public async Task ConfirmEmail()
    {
        var request = new ConfirmEmailCommand
        {
            UserId = DbUserFixtures.ExistingUserWithoutConfirmedEmail.Id.Value,
            Token = DbUserFixtures.ExistingUserWithoutConfirmedEmail.EmailConfirmToken!.Value,
        };
        
        var response = await _httpClient.PostAsJsonAsync(AccountV1Routes.ConfirmEmail, request);

        await Assert.SuccessEmptyResponseAsync(response);
    }
    
    [Fact]
    public async Task ConfirmEmailAfterExpiration()
    {
        var request = new ConfirmEmailCommand
        {
            UserId = DbUserFixtures.ExistingUserWithExpiredToken.Id.Value,
            Token = DbUserFixtures.ExistingUserWithExpiredToken.EmailConfirmToken!.Value,
        };
        
        var response = await _httpClient.PostAsJsonAsync(AccountV1Routes.ConfirmEmail, request);

        await Assert.ConflictResponseAsync(response);
    }
    
    [Fact]
    public async Task ConfirmEmailWithInvalidToken()
    {
        var request = new ConfirmEmailCommand
        {
            UserId = DbUserFixtures.ExistingUserWithExpiredToken.Id.Value,
            Token = "invalid-token",
        };
        
        var response = await _httpClient.PostAsJsonAsync(AccountV1Routes.ConfirmEmail, request);
        
        await Assert.ConflictResponseAsync(response);
    }
    
    [Fact]
    public async Task ConfirmEmailWithEmpty()
    {
        var request = new ConfirmEmailCommand
        {
            UserId = Guid.Empty,
            Token = string.Empty
        };
        
        var response = await _httpClient.PostAsJsonAsync(AccountV1Routes.ConfirmEmail, request);
        
        await Assert.ValidationResponseAsync(response, expectedErrors: 2);
    }
}