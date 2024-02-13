using System.Net.Http.Json;
using JoBoard.AuthService.Tests.Functional.Fixtures;
using Assert = JoBoard.AuthService.Tests.Functional.Extensions.Assert;

namespace JoBoard.AuthService.Tests.Functional.Tests.API.ManageAccountV1Controller;

public class DeactivateAccountTests : BaseApiTest
{
    public DeactivateAccountTests(
        CustomWebApplicationFactory webApplicationFactory, 
        DatabaseFixture databaseFixture, 
        RedisFixture redisFixture) : base(webApplicationFactory, databaseFixture, redisFixture) { }
    
    // TODO add tests
    
    [Fact]
    public async Task RequestAccountDeactivationUnauthorized()
    {
        var response = await HttpClient.PostAsJsonAsync(ManageAccountV1Routes.RequestAccountDeactivation, new {});

        await Assert.UnauthorizedResponseAsync(response);
    }
    
    [Fact]
    public async Task ConfirmAccountDeactivationUnauthorized()
    {
        var response = await HttpClient.PostAsJsonAsync(ManageAccountV1Routes.ConfirmAccountDeactivation, new {});

        await Assert.UnauthorizedResponseAsync(response);
    }
}