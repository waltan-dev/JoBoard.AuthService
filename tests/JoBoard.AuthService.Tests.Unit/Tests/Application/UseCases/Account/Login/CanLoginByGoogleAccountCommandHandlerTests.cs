using JoBoard.AuthService.Application.Commands.Login.CanLoginByGoogle;
using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Tests.Unit.Fixtures;

namespace JoBoard.AuthService.Tests.Unit.Tests.Application.UseCases.Account.Login;

public class CanLoginByGoogleAccountCommandHandlerTests
{
    [Fact]
    public async Task HandleValid()
    {
        var command = new CanLoginByGoogleAccountCommand
        {
            GoogleIdToken = GoogleFixtures.IdTokenForExistingUser
        };
        
        var handler = CreateHandler();
        var result = await handler.Handle(command, default);
        
        Assert.Equal(GoogleFixtures.UserProfileForExistingUser.Email.ToLower(), result.Email);
    }
    
    [Fact]
    public async Task HandleWithInvalidToken()
    {
        var command = new CanLoginByGoogleAccountCommand
        {
            GoogleIdToken = "invalid-token"
        };
        
        var handler = CreateHandler();
        await Assert.ThrowsAsync<ValidationException>(async () =>
        {
            await handler.Handle(command, default);
        });
    }
    
    [Fact]
    public async Task HandleWithNonExistingUser()
    {
        var command = new CanLoginByGoogleAccountCommand
        {
            GoogleIdToken = GoogleFixtures.IdTokenForNewUser
        };
        
        var handler = CreateHandler();
        await Assert.ThrowsAsync<NotFoundException>(async () =>
        {
            await handler.Handle(command, default);
        });
    }
    
    private static CanLoginByGoogleAccountCommandHandler CreateHandler()
    {
        return new CanLoginByGoogleAccountCommandHandler(
            UnitTestsRegistry.GoogleAuthProvider,
            UnitTestsRegistry.UserRepository);
    }
}