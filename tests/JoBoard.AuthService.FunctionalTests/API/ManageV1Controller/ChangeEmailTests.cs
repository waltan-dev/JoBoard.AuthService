using System.Net.Http.Json;

namespace JoBoard.AuthService.FunctionalTests.API.ManageV1Controller;

public class ChangeEmailTests: IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    
    public ChangeEmailTests(CustomWebApplicationFactory factory) // Setup for each fact
    {
        _httpClient = factory.CreateClient();
    }
    
    // TODO add tests
    
    [Fact]
    public async Task RequestEmailChangeUnauthorized()
    {
        var response = await _httpClient.PostAsJsonAsync(ManageV1Routes.RequestEmailChange, new {});

        await Assert.UnauthorizedResponseAsync(response);
    }
    
    [Fact]
    public async Task ConfirmEmailChangeUnauthorized()
    {
        var response = await _httpClient.PostAsJsonAsync(ManageV1Routes.ConfirmEmailChange, new {});

        await Assert.UnauthorizedResponseAsync(response);
    }
}