using JoBoard.AuthService.Application.Commands.Login.CanLogin;
using JoBoard.AuthService.Application.Exceptions;
using JoBoard.AuthService.Tests.Unit.Fixtures;

namespace JoBoard.AuthService.Tests.Unit.Tests.Application.UseCases.Account.Login;

public class CanLoginCommandHandlerTests
{
    [Fact]
    public async Task HandleValid()
    {
        var command = new CanLoginCommand
        {
            UserId = DbUserFixtures.ExistingUserWithoutConfirmedEmail.Id.Value.ToString()
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
            UserId = Guid.NewGuid().ToString()
        };
        
        var handler = CreateHandler();
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await handler.Handle(command, default);
        });
    }
    
    private static CanLoginCommandHandler CreateHandler()
    {
        return new CanLoginCommandHandler(UnitTestsRegistry.UserRepository);
    }
}