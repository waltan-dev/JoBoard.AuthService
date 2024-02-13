using JoBoard.AuthService.Application.Common.Configs;
using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Application.UseCases.Account.Register.ByEmailAndPassword;
using JoBoard.AuthService.Tests.Common.DataFixtures;

using JoBoard.AuthService.Tests.Common.Stubs;

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
            Email = DbUserFixtures.ExistingUserWithoutConfirmedEmail.Value.Email.Value,
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
        var passwordValidator = PasswordStrengthValidatorStubFactory.Create();
        var passwordHasher = PasswordHasherStubFactory.Create();
        var tokenizer = SecureTokenizerStubFactory.Create();
        var eventDispatcher = DomainEventDispatcherStubFactory.Create();
        var userRepository = UserRepositoryStubFactory.Create();
        var confirmTokenConfig = new ConfirmationTokenConfig { TokenLifeSpan = TimeSpan.FromHours(24) };
        
        return new RegisterByEmailAndPasswordCommandHandler(
            passwordValidator, 
            tokenizer,
            eventDispatcher,
            userRepository,
            passwordHasher,
            confirmTokenConfig);
    }
}