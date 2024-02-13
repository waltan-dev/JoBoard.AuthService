using JoBoard.AuthService.Application.Common.Services;
using JoBoard.AuthService.Domain.Aggregates.UserAggregate;
using JoBoard.AuthService.Domain.Common.SeedWork;
using JoBoard.AuthService.Domain.Common.Services;
using JoBoard.AuthService.Infrastructure.Common.Services;
using JoBoard.AuthService.Tests.Common.Builders;
using JoBoard.AuthService.Tests.Common.Fixtures;
using JoBoard.AuthService.Tests.Common.Stubs;
using JoBoard.AuthService.Tests.Unit.Fixtures;
using JoBoard.AuthService.Tests.Unit.Stubs;

namespace JoBoard.AuthService.Tests.Unit;

public static class UnitTestsRegistry
{
    public static IPasswordHasher PasswordHasher 
        => new PasswordHasherStubFactory().Create();
    
    public static IPasswordStrengthValidator PasswordStrengthValidator 
        => new PasswordStrengthValidatorStubFactory().Create();
    
    public static IUserEmailUniquenessChecker UserEmailUniquenessChecker 
        => new UserEmailUniquenessCheckerStubFactory().Create(DbUserFixtures.List);
    
    public static IExternalAccountUniquenessChecker ExternalAccountUniquenessChecker 
        => new ExternalAccountUniquenessCheckerStubFactory().Create(DbUserFixtures.List);
    
    public static ISecureTokenizer SecureTokenizer
        => new SecureTokenizerStubFactory().Create();
    
    public static IDomainEventDispatcher DomainEventDispatcher
        => new DomainEventDispatcherStubFactory().Create();
    
    public static IGoogleAuthProvider GoogleAuthProvider
        => new GoogleAuthProviderStubFactory().Create(GoogleFixtures.Dictionary);
    
    public static ConfirmationTokenBuilder ConfirmationTokenBuilder 
        => new(SecureTokenizer);
    
    public static UserPasswordBuilder UserPasswordBuilder => new(PasswordHasher, PasswordStrengthValidator);

    public static UserBuilder UserBuilder =>
        new(PasswordFixtures.DefaultPassword,
            ConfirmationTokenBuilder,
            PasswordHasher,
            PasswordStrengthValidator);

    public static IUserRepository UserRepository => UserRepositoryStubFactory.Create(DbUserFixtures.List);
    
    public static IDateTime CurrentDateTime => new DateTimeProvider();
    public static IDateTime FutureDateTime => new TestDateTimeProvider(DateTime.UtcNow.AddDays(7));
}