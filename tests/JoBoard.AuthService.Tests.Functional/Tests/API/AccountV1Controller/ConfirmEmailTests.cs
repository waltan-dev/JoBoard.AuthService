using System.Net.Http.Json;
using JoBoard.AuthService.Application.Commands.ConfirmEmail;
using JoBoard.AuthService.Tests.Functional.Fixtures;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Assert = JoBoard.AuthService.Tests.Functional.Extensions.Assert;

namespace JoBoard.AuthService.Tests.Functional.Tests.API.AccountV1Controller;

public class ConfirmEmailTests : BaseApiTest
{
    public ConfirmEmailTests(
        CustomWebApplicationFactory webApplicationFactory, 
        DatabaseFixture databaseFixture, 
        RedisFixture redisFixture) : base(webApplicationFactory, databaseFixture, redisFixture) { }
    
    [Fact]
    public async Task CheckSuccess()
    {
        // Arrange
        var request = new ConfirmEmailCommand
        {
            UserId = DbUserFixtures.ExistingUserWithoutConfirmedEmail01.Id.Value,
            Token = DbUserFixtures.ExistingUserWithoutConfirmedEmail01.EmailConfirmToken!.Value
        };
        
        // Act
        var response = await HttpClient.PostAsJsonAsync(AccountV1Routes.ConfirmEmail, request);

        // Assert
        await Assert.SuccessEmptyResponseAsync(response);
    }
    
    [Fact]
    public async Task CheckConflict()
    {
        // Arrange
        var serverWithFutureDate = GetServerWithFutureDate();
        var httpClient = serverWithFutureDate.CreateClient();
        
        var request = new ConfirmEmailCommand
        {
            UserId = DbUserFixtures.ExistingUserWithoutConfirmedEmail02.Id.Value,
            Token = DbUserFixtures.ExistingUserWithoutConfirmedEmail02.EmailConfirmToken!.Value
        };
        
        // Act
        var response = await httpClient.PostAsJsonAsync(AccountV1Routes.ConfirmEmail, request);

        // Assert
        await Assert.ConflictResponseAsync(response);
    }
    
    [Fact]
    public async Task CheckValidation()
    {
        // Arrange
        var request = new ConfirmEmailCommand
        {
            UserId = Guid.Empty,
            Token = string.Empty
        };
        
        // Act
        var response = await HttpClient.PostAsJsonAsync(AccountV1Routes.ConfirmEmail, request);
        
        // Assert
        await Assert.ValidationResponseAsync(response, expectedErrors: 2);
    }
}