using JoBoard.AuthService.Application.Contracts;
using JoBoard.AuthService.Domain.SeedWork;
using JoBoard.AuthService.Domain.Services;
using JoBoard.AuthService.Tests.Common;
using JoBoard.AuthService.Tests.Common.Builders;
using JoBoard.AuthService.Tests.Common.Fixtures;
using JoBoard.AuthService.Tests.Common.Stubs;
using JoBoard.AuthService.Tests.Functional.Fixtures;

namespace JoBoard.AuthService.Tests.Functional;

public static class FunctionalTestsRegistry
{
    private static IPasswordHasher PasswordHasher 
        => new PasswordHasherStubFactory().Create();

    private static IPasswordStrengthValidator PasswordStrengthValidator 
        => new PasswordStrengthValidatorStubFactory().Create();

    private static ISecureTokenizer SecureTokenizer
        => new SecureTokenizerStubFactory().Create();
    
    public static ConfirmationTokenBuilder ConfirmationTokenBuilder 
        => new (SecureTokenizer);
    
    public static IGoogleAuthProvider GoogleAuthProvider 
        => new GoogleAuthProviderStubFactory().Create(GoogleFixtures.Dictionary);
    
    public static UserBuilder UserBuilder =>
        new(PasswordFixtures.DefaultPassword,
            ConfirmationTokenBuilder,
            PasswordHasher,
            PasswordStrengthValidator);

    public static IDateTime CurrentDateTime => new DateTimeProvider();
    public static IDateTime FutureDateTime => new TestDateTimeProvider(DateTime.UtcNow.AddDays(7));
}