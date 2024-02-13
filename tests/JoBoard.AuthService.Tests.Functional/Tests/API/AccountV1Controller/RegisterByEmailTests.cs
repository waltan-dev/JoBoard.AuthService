using System.Net.Http.Json;
using JoBoard.AuthService.Application.Commands.Register.ByEmailAndPassword;
using JoBoard.AuthService.Tests.Functional.Fixtures;
using Assert = JoBoard.AuthService.Tests.Functional.Extensions.Assert;

namespace JoBoard.AuthService.Tests.Functional.Tests.API.AccountV1Controller;

public class RegisterByEmailTests : BaseApiTest
{
    public RegisterByEmailTests(
        CustomWebApplicationFactory webApplicationFactory, 
        DatabaseFixture databaseFixture, 
        RedisFixture redisFixture) : base(webApplicationFactory, databaseFixture, redisFixture) { }
    
    [Fact]
    public async Task CheckSuccess()
    {
        // Arrange
        var request = new RegisterByEmailAndPasswordCommand
        {
            FirstName = "Test",
            LastName = "Test",
            Email = " test@gmail.com ",
            Password = "ValidPassword123$",
            Role = " Hirer "
        };
        
        // Act
        var response = await HttpClient.PostAsJsonAsync(AccountV1Routes.Register, request);
        
        // Assert
        await Assert.SuccessEmptyResponseAsync(response);
    }

    [Fact]
    public async Task CheckConflict() 
    {
        // no case for conflict
    }
    
    [Fact]
    public async Task CheckValidation()
    {
        // Arrange
        var request = new RegisterByEmailAndPasswordCommand
        {
            FirstName = "  ", LastName = "  ", Email = "invalid-email", Password = "1", Role = "invalid-role"
        };
        
        // Act
        var response = await HttpClient.PostAsJsonAsync(AccountV1Routes.Register, request);
        
        // Assert
        await Assert.ValidationResponseAsync(response, expectedErrors: 5);
    }
}