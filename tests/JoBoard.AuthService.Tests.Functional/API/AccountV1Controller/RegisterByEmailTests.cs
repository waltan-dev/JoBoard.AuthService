using System.Net.Http.Json;
using JoBoard.AuthService.Application.UseCases.Account.Register.ByEmailAndPassword;
using JoBoard.AuthService.Tests.Common.DataFixtures;


namespace JoBoard.AuthService.Tests.Functional.API.AccountV1Controller;

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
        var request = new RegisterByEmailAndPasswordCommand
        {
            FirstName = "Test",
            LastName = "Test",
            Email = " test@gmail.com ",
            Password = "ValidPassword123$",
            Role = " Hirer "
        };
        
        var response = await _httpClient.PostAsJsonAsync(AccountV1Routes.Register, request);
        
        await Assert.SuccessEmptyResponseAsync(response);
    }
    
    [Fact]
    public async Task RegisterWithExistingEmail()
    {
        var request = new RegisterByEmailAndPasswordCommand
        {
            FirstName = "Test",
            LastName = "Test",
            Email = DbUserFixtures.ExistingUserWithoutConfirmedEmail.Value.Email.Value,
            Password = "ValidPassword123$",
            Role = "Hirer"
        };
        
        var response = await _httpClient.PostAsJsonAsync(AccountV1Routes.Register, request);
        
        await Assert.ValidationResponseAsync(response, 1);
    }

    [Fact]
    public async Task RegisterWithEmpty()
    {
        var request = new RegisterByEmailAndPasswordCommand
        {
            FirstName = "", LastName = "", Email = "", Password = "", Role = ""
        };
        
        var response = await _httpClient.PostAsJsonAsync(AccountV1Routes.Register, request);
        
        await Assert.ValidationResponseAsync(response, expectedErrors: 5);
    }
    
    [Fact]
    public async Task RegisterWithInvalidFields()
    {
        // TODO implement request validation and response
        var request = new RegisterByEmailAndPasswordCommand
        {
            FirstName = "  ", LastName = "  ", Email = "invalid-email", Password = "1", Role = "invalid-role"
        };
        
        var response = await _httpClient.PostAsJsonAsync(AccountV1Routes.Register, request);
        
        await Assert.ValidationResponseAsync(response, expectedErrors: 5);
    }
}