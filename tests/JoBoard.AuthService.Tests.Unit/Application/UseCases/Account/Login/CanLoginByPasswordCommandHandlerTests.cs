using JoBoard.AuthService.Application.UseCases.Account.Login.CanLoginByPassword;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.DataFixtures;

using JoBoard.AuthService.Tests.Common.Stubs;

namespace JoBoard.AuthService.Tests.Unit.Application.UseCases.Account.Login;

public class CanLoginByPasswordCommandHandlerTests
{
    [Fact]
    public async Task HandleValid()
    {
        var command = new CanLoginByPasswordCommand
        {
            Email = DbUserFixtures.ExistingUserWithoutConfirmedEmail.Value.Email.Value,
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
            PasswordHasherStubFactory.Create(),
            UserRepositoryStubFactory.Create());
    }
}