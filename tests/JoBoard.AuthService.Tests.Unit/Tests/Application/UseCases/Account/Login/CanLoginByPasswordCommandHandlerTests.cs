using JoBoard.AuthService.Application.Commands.Login.CanLoginByPassword;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Tests.Common.Fixtures;
using JoBoard.AuthService.Tests.Unit.Fixtures;

namespace JoBoard.AuthService.Tests.Unit.Tests.Application.UseCases.Account.Login;

public class CanLoginByPasswordCommandHandlerTests
{
    [Fact]
    public async Task HandleValid()
    {
        var command = new CanLoginByPasswordCommand
        {
            Email = DbUserFixtures.ExistingUserWithoutConfirmedEmail.Email.Value,
            Password = PasswordFixtures.DefaultPassword
        };
        
        var handler = CreateHandler();
        var result = await handler.Handle(command, default);
        
        Assert.Equal(command.Email, result.Email);
    }
    
    [Fact]
    public async Task HandleWithNonExistingUser()
    {
        var command = new CanLoginByPasswordCommand
        {
            Email = "someEmail@gmail.com",
            Password = "somePassword"
        };
        
        var handler = CreateHandler();
        await Assert.ThrowsAsync<DomainException>(async () =>
        {
            await handler.Handle(command, default);
        });
    }
    
    private static CanLoginByPasswordCommandHandler CreateHandler()
    {
        return new CanLoginByPasswordCommandHandler(
            UnitTestsRegistry.PasswordHasher,
            UnitTestsRegistry.UserRepository);
    }
}