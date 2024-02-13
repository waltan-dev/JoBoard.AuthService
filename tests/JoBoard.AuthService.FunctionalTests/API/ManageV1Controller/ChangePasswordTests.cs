using System.Net.Http.Json;
using JoBoard.AuthService.Application.UseCases.Manage.ChangePassword;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.FunctionalTests.API.ManageV1Controller;

public class ChangePasswordTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    
    public ChangePasswordTests(CustomWebApplicationFactory factory) // Setup for each fact
    {
        _httpClient = factory.CreateClient();
    }
    
    [Fact]
    public async Task ChangePassword()
    {
        var request = new ChangePasswordCommand
        {
            CurrentPassword = PasswordFixtures.DefaultPassword,
            NewPassword = PasswordFixtures.NewPassword
        };

        await _httpClient.AuthorizeAsync(DatabaseUserFixtures.ExistingUserRegisteredByEmail);
        var response = await _httpClient.PostAsJsonAsync(ManageV1Routes.ChangePassword, request);

        await Assert.SuccessEmptyResponseAsync(response);
    }
    
    // TODO add test with invalid current password
    // TODO add test with non-strength new password
    
    [Fact]
    public async Task Unauthorized()
    {
        var response = await _httpClient.PostAsJsonAsync(ManageV1Routes.ChangePassword, new {});

        await Assert.UnauthorizedResponseAsync(response);
    }
}