using System.Net.Http.Json;
using JoBoard.AuthService.Tests.Functional.Fixtures;
using Assert = JoBoard.AuthService.Tests.Functional.Extensions.Assert;

namespace JoBoard.AuthService.Tests.Functional.Tests.API.ManageAccountV1Controller;

public class ChangeRoleTests : BaseApiTest
{
    public ChangeRoleTests(
        CustomWebApplicationFactory webApplicationFactory, 
        DatabaseFixture databaseFixture, 
        RedisFixture redisFixture) : base(webApplicationFactory, databaseFixture, redisFixture) { }
    
    // TODO add tests
    
    [Fact]
    public async Task ChangeRoleUnauthorized()
    {
        var response = await HttpClient.PostAsJsonAsync(ManageAccountV1Routes.ChangeRole, new {});

        await Assert.UnauthorizedResponseAsync(response);
    }
}