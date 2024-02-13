using System.Net.Http.Json;
using JoBoard.AuthService.Application.UseCases.Auth.ConfirmEmail;
using JoBoard.AuthService.Tests.Common;

namespace JoBoard.AuthService.FunctionalTests.API.AuthV1Controller;

public class ConfirmEmailTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient; // one per fact
    
    public ConfirmEmailTests(CustomWebApplicationFactory factory) // SetUp
    {
        factory.ResetDatabase();
        _httpClient = factory.CreateClient();
    }
    
    [Fact]
    public async Task ConfirmEmail()
    {
        var request = new ConfirmEmailCommand
        {
            UserId = UserFixtures.ExistingUserRegisteredByEmail.Id.Value,
            Token = UserFixtures.ExistingUserRegisteredByEmail.RegisterConfirmToken!.Value,
        };
        
        var response = await _httpClient.PostAsJsonAsync(AuthV1Routes.ConfirmEmail, request);

        await Assert.SuccessEmptyResponseAsync(response);
    }
    
    [Fact]
    public async Task ConfirmEmailAfterExpiration()
    {
        var request = new ConfirmEmailCommand
        {
            UserId = UserFixtures.ExistingUserWithExpiredToken.Id.Value,
            Token = UserFixtures.ExistingUserWithExpiredToken.RegisterConfirmToken!.Value,
        };
        
        var response = await _httpClient.PostAsJsonAsync(AuthV1Routes.ConfirmEmail, request);

        await Assert.ConflictResponseAsync(response);
    }
    
    [Fact]
    public async Task ConfirmEmailWithInvalidToken()
    {
        var request = new ConfirmEmailCommand
        {
            UserId = UserFixtures.ExistingUserWithExpiredToken.Id.Value,
            Token = "invalid-token",
        };
        
        var response = await _httpClient.PostAsJsonAsync(AuthV1Routes.ConfirmEmail, request);
        
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
        
        var response = await _httpClient.PostAsJsonAsync(AuthV1Routes.ConfirmEmail, request);
        
        await Assert.ValidationResponseAsync(response, expectedErrors: 2);
    }
}