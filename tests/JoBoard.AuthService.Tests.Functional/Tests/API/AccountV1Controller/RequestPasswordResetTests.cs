using System.Net.Http.Json;
using JoBoard.AuthService.Application.Commands.ResetPassword.Request;
using JoBoard.AuthService.Tests.Functional.Fixtures;
using Assert = JoBoard.AuthService.Tests.Functional.Extensions.Assert;

namespace JoBoard.AuthService.Tests.Functional.Tests.API.AccountV1Controller;

public class RequestPasswordResetTests : BaseApiTest
{
    public RequestPasswordResetTests(
        CustomWebApplicationFactory webApplicationFactory, 
        DatabaseFixture databaseFixture, 
        RedisFixture redisFixture) : base(webApplicationFactory, databaseFixture, redisFixture) { }
    
    [Fact]
    public async Task CheckSuccess()
    {
        // Arrange
        var request = new RequestPasswordResetCommand()
        {
            Email = DbUserFixtures.ExistingActiveUser01.Email.Value
        };
        
        // Act
        var response = await HttpClient.PostAsJsonAsync(AccountV1Routes.RequestPasswordReset, request);

        // Assert
        await Assert.SuccessEmptyResponseAsync(response);
    }
    
    [Fact]
    public async Task CheckNotFound()
    {
        // Arrange
        var request = new RequestPasswordResetCommand
        {
            Email = "not-found-email@gmail.com"
        };
        
        // Act
        var response = await HttpClient.PostAsJsonAsync(AccountV1Routes.RequestPasswordReset, request);

        // Assert
        await Assert.NotFoundResponseAsync(response);
    }
    
    [Fact]
    public async Task CheckConflict()
    {
        // Arrange
        var anotherUser = DbUserFixtures.ExistingActiveUser02;
        var request = new RequestPasswordResetCommand
        {
            Email = anotherUser.Email.Value
        };
        await HttpClient.PostAsJsonAsync(AccountV1Routes.RequestPasswordReset, request);
        
        // Act
        var response = await HttpClient.PostAsJsonAsync(AccountV1Routes.RequestPasswordReset, request);

        // Assert
        await Assert.ConflictResponseAsync(response);
    }
    
    [Fact]
    public async Task CheckValidation()
    {
        // Arrange
        var request = new RequestPasswordResetCommand
        {
            Email = "invalid-email"
        };
        
        // Act
        var response = await HttpClient.PostAsJsonAsync(AccountV1Routes.RequestPasswordReset, request);

        // Assert
        await Assert.ValidationResponseAsync(response, 1);
    }
}