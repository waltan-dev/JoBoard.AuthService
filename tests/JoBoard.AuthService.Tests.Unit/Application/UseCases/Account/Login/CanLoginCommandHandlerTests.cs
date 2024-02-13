using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Application.UseCases.Account.Login.CanLogin;
using JoBoard.AuthService.Application.UseCases.Account.Login.CanLoginByPassword;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Fixtures;

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
        return new CanLoginCommandHandler(
            DatabaseFixtures.CreateUserRepositoryStub());
    }
}