using System.Net.Http.Json;
using JoBoard.AuthService.Application.UseCases.Auth.Register.ByEmail;
using Microsoft.AspNetCore.Http;

namespace JoBoard.AuthService.IntegrationTests.API.Controllers.AccountV1;

public class RegisterByEmailTests : BaseApiTest, IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient; // one per fact
    
    public RegisterByEmailTests(CustomWebApplicationFactory factory) // SetUp
    {
        _httpClient = factory.CreateClient();
    }
    
    [Fact]
    public async Task RegisterByEmail()
    {
        var request = new RegisterByEmailCommand
        {
            FirstName = "Test",
            LastName = "Test",
            Email = " test@gmail.com ",
            Password = "ValidPassword123$",
            Role = " Hirer "
        };
        
        var response = await _httpClient.PostAsJsonAsync("api/v1/account/register", request);
        
        await AssertSuccessEmptyResponseAsync(response, StatusCodes.Status201Created);
    }
    
    [Fact]
    public async Task RegisterWithExistingEmail()
    {
        var request = new RegisterByEmailCommand
        {
            FirstName = "Test",
            LastName = "Test",
            Email = RegisterFixtures.ExistingUserRegisteredByEmail.Email.Value,
            Password = "ValidPassword123$",
            Role = "Hirer"
        };
        
        var response = await _httpClient.PostAsJsonAsync("api/v1/account/register", request);
        
        await AssertConflictResponseAsync(response);
    }

    [Fact]
    public async Task RegisterWithEmpty()
    {
        var request = new RegisterByEmailCommand
        {
            FirstName = "", LastName = "", Email = "", Password = "", Role = ""
        };
        
        var response = await _httpClient.PostAsJsonAsync("api/v1/account/register", request);
        
        await AssertValidationResponseAsync(response, expectedErrors: 5);
    }
    
    [Fact]
    public async Task RegisterWithInvalidFields()
    {
        // TODO implement request validation and response
        var request = new RegisterByEmailCommand
        {
            FirstName = "  ", LastName = "  ", Email = "invalid-email", Password = "1", Role = "invalid-role"
        };
        
        var response = await _httpClient.PostAsJsonAsync("api/v1/account/register", request);
        
        await AssertValidationResponseAsync(response, expectedErrors: 5);
    }
}