using System.Net.Http.Json;

namespace JoBoard.AuthService.FunctionalTests.API.ManageAccountV1Controller;

public class ChangeRoleTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    
    public ChangeRoleTests(CustomWebApplicationFactory factory) // Setup for each fact
    {
        _httpClient = factory.CreateClient();
    }
    
    // TODO add tests
    
    [Fact]
    public async Task ChangeRoleUnauthorized()
    {
        var response = await _httpClient.PostAsJsonAsync(ManageAccountV1Routes.ChangeRole, new {});

        await Assert.UnauthorizedResponseAsync(response);
    }
}