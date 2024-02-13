using JoBoard.AuthService.Application.Commands.Account.Register.ByEmailAndPassword;
using JoBoard.AuthService.Application.Configs;
using JoBoard.AuthService.Application.Exceptions;
using JoBoard.AuthService.Tests.Common.DataFixtures;

namespace JoBoard.AuthService.Tests.Unit.Application.UseCases.Account.Register;

public class RegisterByEmailAndPasswordCommandHandlerTests
{
    [Fact]
    public async Task HandleValid()
    {
        var command = new RegisterByEmailAndPasswordCommand
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            Email = "someUniqueEmail@gmail.com",
            Password = PasswordFixtures.DefaultPassword,
            Role = "Worker"
        };
        
        var handler = CreateHandler();
        var result = await handler.Handle(command, default);
        
        Assert.Equal(result, MediatR.Unit.Value);
    }
    
    [Fact]
    public async Task HandleWithExistingEmail()
    {
        var command = new RegisterByEmailAndPasswordCommand
        {
            FirstName = "Ivan",
            LastName = "Ivanov",
            Email = DbUserFixtures.ExistingUserWithoutConfirmedEmail.Email.Value,
            Password = PasswordFixtures.DefaultPassword,
            Role = "Worker"
        };

        var handler = CreateHandler();
        await Assert.ThrowsAsync<ValidationException>(async() =>
        {
            await handler.Handle(command, default);
        });
    }

    private static RegisterByEmailAndPasswordCommandHandler CreateHandler()
    {
        return new RegisterByEmailAndPasswordCommandHandler(
            TestsRegistry.PasswordStrengthValidator, 
            TestsRegistry.SecureTokenizer,
            TestsRegistry.DomainEventDispatcher,
            TestsRegistry.UserRepository,
            TestsRegistry.PasswordHasher,
            TestsRegistry.UserEmailUniquenessChecker,
            new ConfirmationTokenConfig { TokenLifeSpan = TimeSpan.FromHours(24) });
    }
}