using System.Net.Http.Json;
using JoBoard.AuthService.Models.Requests;
using JoBoard.AuthService.Tests.Functional.Fixtures;
using Assert = JoBoard.AuthService.Tests.Functional.Extensions.Assert;

namespace JoBoard.AuthService.Tests.Functional.Tests.API.TokenAuthV1Controller;

public class TokenByGoogleTests : BaseApiTest
{
    public TokenByGoogleTests(
        CustomWebApplicationFactory webApplicationFactory, 
        DatabaseFixture databaseFixture, 
        RedisFixture redisFixture) : base(webApplicationFactory, databaseFixture, redisFixture) { }
    
    [Fact]
    public async Task TokenByGoogle()
    {
        var request = new LoginByGoogleRequest
        {
            IdToken = GoogleFixtures.IdTokenForExistingUser
        };
        
        var response = await HttpClient.PostAsJsonAsync(AuthTokenV1Routes.TokenByGoogle, request);
        
        await Assert.SuccessAuthResponseAsync(response);
    }
    
    // TODO add test with empty
    // TODO add test with non existing user
    // TODO add test with invalid token
}