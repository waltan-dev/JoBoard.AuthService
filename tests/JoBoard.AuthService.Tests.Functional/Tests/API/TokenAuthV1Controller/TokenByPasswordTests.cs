using System.Net.Http.Json;
using JoBoard.AuthService.Models.Requests;
using JoBoard.AuthService.Tests.Common.Fixtures;
using JoBoard.AuthService.Tests.Functional.Fixtures;
using Assert = JoBoard.AuthService.Tests.Functional.Extensions.Assert;

namespace JoBoard.AuthService.Tests.Functional.Tests.API.TokenAuthV1Controller;

public class TokenByPasswordTests : BaseApiTest
{
    public TokenByPasswordTests(
        CustomWebApplicationFactory webApplicationFactory, 
        DatabaseFixture databaseFixture, 
        RedisFixture redisFixture) : base(webApplicationFactory, databaseFixture, redisFixture) { }
    
    [Fact]
    public async Task TokenByPassword()
    {
        var request = new LoginByPasswordRequest
        {
            Email = DbUserFixtures.ExistingUserWithoutConfirmedEmail01.Email.Value,
            Password = PasswordFixtures.DefaultPassword,
        };
        
        var response = await HttpClient.PostAsJsonAsync(AuthTokenV1Routes.TokenByPassword, request);
        
        await Assert.SuccessAuthResponseAsync(response);
    }
    
    // TODO add test with empty
    // TODO add test with invalid email
    // TODO add test with invalid password
    // TODO add test with non-existing email
}