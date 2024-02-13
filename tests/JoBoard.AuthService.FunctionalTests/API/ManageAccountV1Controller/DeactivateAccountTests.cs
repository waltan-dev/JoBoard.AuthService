using System.Net.Http.Json;

namespace JoBoard.AuthService.FunctionalTests.API.ManageAccountV1Controller;

public class DeactivateAccountTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    
    public DeactivateAccountTests(CustomWebApplicationFactory factory) // Setup for each fact
    {
        _httpClient = factory.CreateClient();
    }
    
    // TODO add tests
    
    [Fact]
    public async Task RequestAccountDeactivationUnauthorized()
    {
        var response = await _httpClient.PostAsJsonAsync(ManageAccountV1Routes.RequestAccountDeactivation, new {});

        await Assert.UnauthorizedResponseAsync(response);
    }
    
    [Fact]
    public async Task ConfirmAccountDeactivationUnauthorized()
    {
        var response = await _httpClient.PostAsJsonAsync(ManageAccountV1Routes.ConfirmAccountDeactivation, new {});

        await Assert.UnauthorizedResponseAsync(response);
    }
}