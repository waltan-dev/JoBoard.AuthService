using System.Net.Http.Json;
using JoBoard.AuthService.Application.Commands.ManageAccount.ChangePassword;
using JoBoard.AuthService.Tests.Common.DataFixtures;


namespace JoBoard.AuthService.Tests.Functional.API.ManageAccountV1Controller;

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

        await _httpClient.AuthorizeAsync(DbUserFixtures.ExistingUserWithoutConfirmedEmail);
        var response = await _httpClient.PostAsJsonAsync(ManageAccountV1Routes.ChangePassword, request);

        await Assert.SuccessEmptyResponseAsync(response);
    }
    
    // TODO add test with invalid current password
    // TODO add test with non-strength new password
    
    [Fact]
    public async Task Unauthorized()
    {
        var response = await _httpClient.PostAsJsonAsync(ManageAccountV1Routes.ChangePassword, new {});

        await Assert.UnauthorizedResponseAsync(response);
    }
}