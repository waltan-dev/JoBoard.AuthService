using System.Net.Http.Json;
using System.Net.Mime;
using JoBoard.AuthService.Application.Commands.Register.ByEmail;
using Microsoft.AspNetCore.Http;

namespace JoBoard.AuthService.IntegrationTests.API.Controllers.AccountV1;

public class RegisterByEmailTests : IClassFixture<CustomWebApplicationFactory>
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
            Password = "password",
            Role = " Hirer "
        };
        var response = await _httpClient.PostAsJsonAsync("api/v1/account/register", request);
        var responseBody = await response.Content.ReadAsStringAsync();
        var responseContentType = response.Content.Headers.ContentType?.MediaType;
        
        Assert.Equal(StatusCodes.Status201Created, (int)response.StatusCode);
        Assert.Equal(string.Empty, responseBody);
        Assert.Null(responseContentType);
    }
    
    [Fact]
    public async Task RegisterWithExistingEmail()
    {
        var request = new RegisterByEmailCommand
        {
            FirstName = "Test",
            LastName = "Test",
            Email = RegisterFixtures.ExistingUserRegisteredByEmail.Email.Value,
            Password = "password",
            Role = "Hirer"
        };
        var response = await _httpClient.PostAsJsonAsync("api/v1/account/register", request);
        var responseBody = await response.Content.ReadAsStringAsync();
        var responseContentType = response.Content.Headers.ContentType?.MediaType;
        
        Assert.Equal(StatusCodes.Status409Conflict, (int)response.StatusCode);
        Assert.NotEmpty(responseBody);
        Assert.Equal(MediaTypeNames.Application.Json, responseContentType);
    }

    [Fact]
    public async Task RegisterWithEmpty()
    {
        // TODO implement request validation and response
        
        var request = new RegisterByEmailCommand
        {
            FirstName = "", LastName = "", Email = "", Password = "", Role = ""
        };
        var response = await _httpClient.PostAsJsonAsync("api/v1/account/register", request);
        var responseBody = await response.Content.ReadAsStringAsync();
        var responseContentType = response.Content.Headers.ContentType?.MediaType;
        
        Assert.Equal(StatusCodes.Status409Conflict, (int)response.StatusCode);
        Assert.NotEmpty(responseBody);
        Assert.Equal(MediaTypeNames.Application.Json, responseContentType);
    }
    
    [Fact]
    public async Task RegisterWithInvalidFields()
    {
        // TODO implement request validation and response
        
        var request = new RegisterByEmailCommand
        {
            FirstName = "Test", LastName = "Test", Email = "invalid-email", Password = "1", Role = "invalid-role"
        };
        var response = await _httpClient.PostAsJsonAsync("api/v1/account/register", request);
        var responseBody = await response.Content.ReadAsStringAsync();
        var responseContentType = response.Content.Headers.ContentType?.MediaType;
        
        Assert.Equal(StatusCodes.Status409Conflict, (int)response.StatusCode);
        Assert.NotEmpty(responseBody);
        Assert.Equal(MediaTypeNames.Application.Json, responseContentType);
    }
}