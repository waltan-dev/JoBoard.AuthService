using System.Net.Http.Json;
using JoBoard.AuthService.Tests.Functional.Fixtures;
using Assert = JoBoard.AuthService.Tests.Functional.Extensions.Assert;

namespace JoBoard.AuthService.Tests.Functional.Tests.API.ManageAccountV1Controller;

public class ChangeEmailTests: BaseApiTest
{
    public ChangeEmailTests(
        CustomWebApplicationFactory webApplicationFactory, 
        DatabaseFixture databaseFixture, 
        RedisFixture redisFixture) : base(webApplicationFactory, databaseFixture, redisFixture) { }
    
    // TODO add tests
    
    [Fact]
    public async Task RequestEmailChangeUnauthorized()
    {
        var response = await HttpClient.PostAsJsonAsync(ManageAccountV1Routes.RequestEmailChange, new {});

        await Assert.UnauthorizedResponseAsync(response);
    }
    
    [Fact]
    public async Task ConfirmEmailChangeUnauthorized()
    {
        var response = await HttpClient.PostAsJsonAsync(ManageAccountV1Routes.ConfirmEmailChange, new {});

        await Assert.UnauthorizedResponseAsync(response);
    }
}