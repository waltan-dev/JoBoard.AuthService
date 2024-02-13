using JoBoard.AuthService.Application.Commands.Register.ByEmailAndPassword;
using JoBoard.AuthService.Application.Common.Configs;
using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Tests.Common.Fixtures;
using JoBoard.AuthService.Tests.Unit.Fixtures;

namespace JoBoard.AuthService.Tests.Unit.Tests.Application.UseCases.Account.Register;

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
            UnitTestsRegistry.PasswordStrengthValidator, 
            UnitTestsRegistry.SecureTokenizer,
            UnitTestsRegistry.DomainEventDispatcher,
            UnitTestsRegistry.UserRepository,
            UnitTestsRegistry.PasswordHasher,
            UnitTestsRegistry.UserEmailUniquenessChecker,
            new ConfirmationTokenConfig { TokenLifeSpan = TimeSpan.FromHours(24) },
            UnitTestsRegistry.CurrentDateTime);
    }
}