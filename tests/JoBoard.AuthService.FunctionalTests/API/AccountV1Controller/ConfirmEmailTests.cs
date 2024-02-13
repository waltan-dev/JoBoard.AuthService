using System.Net.Http.Json;
using JoBoard.AuthService.Application.UseCases.Account.ConfirmEmail;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.FunctionalTests.API.AccountV1Controller;

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
            UserId = DatabaseUserFixtures.ExistingUserRegisteredByEmail.Value.Id.Value,
            Token = DatabaseUserFixtures.ExistingUserRegisteredByEmail.Value.RegisterConfirmToken!.Value,
        };
        
        var response = await _httpClient.PostAsJsonAsync(AccountV1Routes.ConfirmEmail, request);

        await Assert.SuccessEmptyResponseAsync(response);
    }
    
    [Fact]
    public async Task ConfirmEmailAfterExpiration()
    {
        var request = new ConfirmEmailCommand
        {
            UserId = DatabaseUserFixtures.ExistingUserWithExpiredToken.Value.Id.Value,
            Token = DatabaseUserFixtures.ExistingUserWithExpiredToken.Value.RegisterConfirmToken!.Value,
        };
        
        var response = await _httpClient.PostAsJsonAsync(AccountV1Routes.ConfirmEmail, request);

        await Assert.ConflictResponseAsync(response);
    }
    
    [Fact]
    public async Task ConfirmEmailWithInvalidToken()
    {
        var request = new ConfirmEmailCommand
        {
            UserId = DatabaseUserFixtures.ExistingUserWithExpiredToken.Value.Id.Value,
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