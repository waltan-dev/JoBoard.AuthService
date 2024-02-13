using System.Net.Http.Json;
using JoBoard.AuthService.Application.Commands.ChangePassword;
using JoBoard.AuthService.Tests.Common.Fixtures;
using JoBoard.AuthService.Tests.Functional.Extensions;
using JoBoard.AuthService.Tests.Functional.Fixtures;
using Assert = JoBoard.AuthService.Tests.Functional.Extensions.Assert;

namespace JoBoard.AuthService.Tests.Functional.Tests.API.ManageAccountV1Controller;

public class ChangePasswordTests : BaseApiTest
{
    public ChangePasswordTests(
        CustomWebApplicationFactory webApplicationFactory, 
        DatabaseFixture databaseFixture, 
        RedisFixture redisFixture) : base(webApplicationFactory, databaseFixture, redisFixture) { }
    
    [Fact]
    public async Task ChangePassword()
    {
        var request = new ChangePasswordCommand
        {
            CurrentPassword = PasswordFixtures.DefaultPassword,
            NewPassword = PasswordFixtures.NewPassword
        };

        await HttpClient.AuthorizeAsync(DbUserFixtures.ExistingUserWithoutConfirmedEmail01);
        var response = await HttpClient.PostAsJsonAsync(ManageAccountV1Routes.ChangePassword, request);

        await Assert.SuccessEmptyResponseAsync(response);
    }
    
    // TODO add test with invalid current password
    // TODO add test with non-strength new password
    
    [Fact]
    public async Task Unauthorized()
    {
        var response = await HttpClient.PostAsJsonAsync(ManageAccountV1Routes.ChangePassword, new {});

        await Assert.UnauthorizedResponseAsync(response);
    }
}