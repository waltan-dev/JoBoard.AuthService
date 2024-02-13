using JoBoard.AuthService.Application.Commands.Register.ByGoogle;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Tests.Unit.Fixtures;

namespace JoBoard.AuthService.Tests.Unit.Tests.Application.UseCases.Account.Register;

public class RegisterByGoogleAccountCommandHandlerTests
{
    [Fact]
    public async Task HandleValid()
    {
        var command = new RegisterByGoogleAccountCommand
        {
            GoogleIdToken = GoogleFixtures.IdTokenForNewUser,
            Role = "Worker"
        };
        
        var handler = CreateHandler();
        var result = await handler.Handle(command, default);
        
        Assert.Equal(result, MediatR.Unit.Value);
    }
    
    [Fact]
    public async Task HandleWithExistingAccount()
    {
        var command = new RegisterByGoogleAccountCommand
        {
            GoogleIdToken = GoogleFixtures.IdTokenForExistingUser,
            Role = "Worker"
        };
        
        var handler = CreateHandler();

        await Assert.ThrowsAsync<DomainException>(async () =>
        {
            _ = await handler.Handle(command, default);
        });
    }
    
    [Fact]
    public async Task HandleWithExistingEmail()
    {
        var command = new RegisterByGoogleAccountCommand
        {
            GoogleIdToken = GoogleFixtures.IdTokenForNewUserWithExistingEmail,
            Role = "Worker"
        };

        var handler = CreateHandler();
        await Assert.ThrowsAsync<DomainException>(async() =>
        {
            await handler.Handle(command, default);
        });
    }
    
    private static RegisterByGoogleAccountCommandHandler CreateHandler()
    {
        return new RegisterByGoogleAccountCommandHandler(
            UnitTestsRegistry.DomainEventDispatcher,
            UnitTestsRegistry.GoogleAuthProvider,
            UnitTestsRegistry.UserRepository,
            UnitTestsRegistry.UserEmailUniquenessChecker,
            UnitTestsRegistry.ExternalAccountUniquenessChecker);
    }
}