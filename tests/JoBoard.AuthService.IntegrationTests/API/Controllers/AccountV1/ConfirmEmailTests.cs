using System.Net.Http.Json;
using JoBoard.AuthService.Application.Commands.ConfirmEmail;
using Microsoft.AspNetCore.Http;

namespace JoBoard.AuthService.IntegrationTests.API.Controllers.AccountV1;

public class ConfirmEmailTests : IClassFixture<CustomWebApplicationFactory>
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
        var responseBody = await response.Content.ReadAsStringAsync();
        var responseContentType = response.Content.Headers.ContentType?.MediaType;
        
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
        Assert.Equal(string.Empty, responseBody);
        Assert.Null(responseContentType);
    }
}