using System.Net.Http.Json;
using JoBoard.AuthService.Application.UseCases.Auth.Register.ByEmail;
using JoBoard.AuthService.Tests.Common;
using JoBoard.AuthService.Tests.Common.Fixtures;

namespace JoBoard.AuthService.FunctionalTests.API.AuthV1Controller;

public class RegisterByEmailTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    
    public RegisterByEmailTests(CustomWebApplicationFactory factory) // Setup for each fact
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
        
        var response = await _httpClient.PostAsJsonAsync(AuthV1Routes.Register, request);
        
        await Assert.SuccessAuthResponseAsync(response);
    }
    
    [Fact]
    public async Task RegisterWithExistingEmail()
    {
        var request = new RegisterByEmailCommand
        {
            FirstName = "Test",
            LastName = "Test",
            Email = DatabaseUserFixtures.ExistingUserRegisteredByEmail.Email.Value,
            Password = "ValidPassword123$",
            Role = "Hirer"
        };
        
        var response = await _httpClient.PostAsJsonAsync(AuthV1Routes.Register, request);
        
        await Assert.ConflictResponseAsync(response);
    }

    [Fact]
    public async Task RegisterWithEmpty()
    {
        var request = new RegisterByEmailCommand
        {
            FirstName = "", LastName = "", Email = "", Password = "", Role = ""
        };
        
        var response = await _httpClient.PostAsJsonAsync(AuthV1Routes.Register, request);
        
        await Assert.ValidationResponseAsync(response, expectedErrors: 5);
    }
    
    [Fact]
    public async Task RegisterWithInvalidFields()
    {
        // TODO implement request validation and response
        var request = new RegisterByEmailCommand
        {
            FirstName = "  ", LastName = "  ", Email = "invalid-email", Password = "1", Role = "invalid-role"
        };
        
        var response = await _httpClient.PostAsJsonAsync(AuthV1Routes.Register, request);
        
        await Assert.ValidationResponseAsync(response, expectedErrors: 5);
    }
}