using System.Net.Http.Json;
using JoBoard.AuthService.Application.UseCases.Account.ResetPassword.Request;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.Tests.Functional.API.AccountV1Controller;

public class RequestPasswordResetTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    
    public RequestPasswordResetTests(CustomWebApplicationFactory factory) // Setup for each fact
    {
        _httpClient = factory.CreateClient();
    }
    
    [Fact]
    public async Task RequestPasswordReset()
    {
        var request = new RequestPasswordResetCommand()
        {
            Email = DbUserFixtures.ExistingActiveUser.Email.Value
        };
        
        var response = await _httpClient.PostAsJsonAsync(AccountV1Routes.RequestPasswordReset, request);

        await Assert.SuccessEmptyResponseAsync(response);
    }
    
    [Fact]
    public async Task RequestPasswordResetWithoutConfirmedEmail()
    {
        var request = new RequestPasswordResetCommand()
        {
            Email = DbUserFixtures.ExistingUserWithoutConfirmedEmail.Value.Email.Value
        };
        
        var response = await _httpClient.PostAsJsonAsync(AccountV1Routes.RequestPasswordReset, request);

        await Assert.ConflictResponseAsync(response);
    }
    
    // TODO add test with duplicate request
    // TODO add test with empty
    // TODO add test with invalid email
}