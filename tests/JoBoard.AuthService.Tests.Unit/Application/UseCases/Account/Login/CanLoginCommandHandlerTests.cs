using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Application.UseCases.Account.Login.CanLogin;
using JoBoard.AuthService.Tests.Common.DataFixtures;
using JoBoard.AuthService.Tests.Common.Stubs;

namespace JoBoard.AuthService.Tests.Unit.Application.UseCases.Account.Login;

public class CanLoginCommandHandlerTests
{
    [Fact]
    public async Task HandleValid()
    {
        var command = new CanLoginCommand
        {
            UserId = DbUserFixtures.ExistingUserWithoutConfirmedEmail.Value.Id.Value
        };
        
        var handler = CreateHandler();
        var result = await handler.Handle(command, default);
        
        Assert.Equal(command.UserId, result.UserId);
    }
    
    [Fact]
    public async Task HandleWithNonExistingUser()
    {
        var command = new CanLoginCommand
        {
            UserId = Guid.NewGuid()
        };
        
        var handler = CreateHandler();
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await handler.Handle(command, default);
        });
    }
    
    private static CanLoginCommandHandler CreateHandler()
    {
        return new CanLoginCommandHandler(UserRepositoryStubFactory.Create());
    }
}