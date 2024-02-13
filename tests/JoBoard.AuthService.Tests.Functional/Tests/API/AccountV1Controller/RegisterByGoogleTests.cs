using System.Net.Http.Json;
using JoBoard.AuthService.Application.Commands.Register.ByGoogle;
using JoBoard.AuthService.Tests.Functional.Fixtures;
using Assert = JoBoard.AuthService.Tests.Functional.Extensions.Assert;

namespace JoBoard.AuthService.Tests.Functional.Tests.API.AccountV1Controller;

public class RegisterByGoogleTests : BaseApiTest
{
    public RegisterByGoogleTests(
        CustomWebApplicationFactory webApplicationFactory, 
        DatabaseFixture databaseFixture, 
        RedisFixture redisFixture) : base(webApplicationFactory, databaseFixture, redisFixture) { }
    
    [Fact]
    public async Task CheckSuccess()
    {
        // Arrange
        var request = new RegisterByGoogleAccountCommand
        {
            GoogleIdToken = GoogleFixtures.IdTokenForNewUser,
            Role = "Hirer"
        };
        
        // Act
        var response = await HttpClient.PostAsJsonAsync(AccountV1Routes.RegisterByGoogle, request);
        
        // Assert
        await Assert.SuccessEmptyResponseAsync(response);
    }
    
    [Fact]
    public async Task CheckConflict()
    {
        // Arrange
        var request = new RegisterByGoogleAccountCommand
        {
            GoogleIdToken = GoogleFixtures.IdTokenForExistingUser,
            Role = "Hirer"
        };
        
        // Act
        var response = await HttpClient.PostAsJsonAsync(AccountV1Routes.RegisterByGoogle, request);
        
        // Assert
        await Assert.ConflictResponseAsync(response);
    }
    
    [Fact]
    public async Task CheckValidation()
    {
        // Arrange
        var request = new RegisterByGoogleAccountCommand
        {
            GoogleIdToken = "invalid-token",
            Role = "Hirer"
        };
        
        // Act
        var response = await HttpClient.PostAsJsonAsync(AccountV1Routes.RegisterByGoogle, request);
        
        // Assert
        await Assert.ValidationResponseAsync(response, 1);
    }
}