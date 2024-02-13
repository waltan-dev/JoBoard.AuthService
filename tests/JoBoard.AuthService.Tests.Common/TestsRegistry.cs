using JoBoard.AuthService.Application.Services;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Domain.Common.Services;
using JoBoard.AuthService.Tests.Common.Builders;
using JoBoard.AuthService.Tests.Common.Stubs;

namespace JoBoard.AuthService.Tests.Common;

public static class TestsRegistry
{
    public static UserBuilder UserBuilder
    {
        get
        {
            return new UserBuilder(ConfirmationTokenBuilder,
                PasswordHasher,
                PasswordStrengthValidator,
                UserEmailUniquenessChecker,
                ExternalAccountUniquenessChecker);
        }
    }

    public static IUserRepository UserRepository
    {
        get
        {
            return UserRepositoryStubFactory.Create();
        }
    }

    public static ConfirmationTokenBuilder ConfirmationTokenBuilder => new();
    public static UserPasswordBuilder UserPasswordBuilder = new();
    
    public static readonly IPasswordHasher PasswordHasher 
        = PasswordHasherStubFactory.Create();
    public static readonly IPasswordStrengthValidator PasswordStrengthValidator 
        = PasswordStrengthValidatorStubFactory.Create();
    public static readonly IUserEmailUniquenessChecker UserEmailUniquenessChecker 
        = UserEmailUniquenessCheckerStubFactory.Create();
    public static readonly IExternalAccountUniquenessChecker ExternalAccountUniquenessChecker 
        = ExternalAccountUniquenessCheckerStubFactory.Create();
    public static readonly ISecureTokenizer SecureTokenizer
        = SecureTokenizerStubFactory.Create();
    public static readonly IDomainEventDispatcher DomainEventDispatcher
        = DomainEventDispatcherStubFactory.Create();
    public static readonly IGoogleAuthProvider GoogleAuthProvider
        = GoogleAuthProviderStubFactory.Create();
}