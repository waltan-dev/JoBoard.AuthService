using System.Net.Http.Json;
using System.Net.Mime;
using JoBoard.AuthService.Application.Commands.Register.ByEmail;
using Microsoft.AspNetCore.Http;

namespace JoBoard.AuthService.IntegrationTests.API.Controllers;

public class AccountV1ControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public AccountV1ControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task Register()
    {
        var client = _factory.CreateClient();

        var request = new RegisterByEmailCommand
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@gmail.com",
            Password = "password",
            Role = "Hirer"
        };
        var response = await client.PostAsJsonAsync("api/v1/account/register", request);
        var responseBody = await response.Content.ReadAsStringAsync();
        var responseContentType = response.Content.Headers.ContentType?.MediaType;
        
        Assert.Equal(StatusCodes.Status201Created, (int)response.StatusCode);
        Assert.Equal(string.Empty, responseBody);
        Assert.Null(responseContentType);
    }
    
    [Fact]
    public async Task RegisterWithExistingEmail()
    {
        var client = _factory.CreateClient();

        var request = new RegisterByEmailCommand
        {
            FirstName = "Test",
            LastName = "Test",
            Email = SeedTestData.User1Hirer.Email.Value,
            Password = "password",
            Role = "Hirer"
        };
        var response = await client.PostAsJsonAsync("api/v1/account/register", request);
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
        
        var client = _factory.CreateClient();

        var request = new RegisterByEmailCommand
        {
            FirstName = "", LastName = "", Email = "", Password = "", Role = ""
        };
        var response = await client.PostAsJsonAsync("api/v1/account/register", request);
        var responseBody = await response.Content.ReadAsStringAsync();
        var responseContentType = response.Content.Headers.ContentType?.MediaType;
        
        Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
        Assert.NotEmpty(responseBody);
        Assert.Equal(MediaTypeNames.Application.Json, responseContentType);
    }
}