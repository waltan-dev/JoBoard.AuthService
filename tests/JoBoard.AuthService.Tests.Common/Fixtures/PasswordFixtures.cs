using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;
using JoBoard.AuthService.Domain.Common.Services;
using JoBoard.AuthService.Infrastructure.Auth.Services;

namespace JoBoard.AuthService.Tests.Common.Fixtures;

public static class PasswordFixtures
{
    public static string DefaultPassword = "ValidPassword123#";
    public static string NewPassword = "ValidPassword123#";
    
    public static IPasswordHasher GetPasswordHasherStub()
    {
        return new PasswordHasher();
        
        // var passwordHasherStub = new Mock<IPasswordHasher>();
        //
        // passwordHasherStub
        //     .Setup(x => x.Hash(NewPassword))
        //     .Returns(NewPasswordHash); // return new hash for new password
        //
        // passwordHasherStub
        //     .Setup(x => x.Verify(DefaultPasswordHash, DefaultPassword))
        //     .Returns(true); // verifies current password before change to new
        // return passwordHasherStub.Object;
    }

    public static IPasswordStrengthValidator GetPasswordStrengthValidatorStub()
    {
        return new PasswordStrengthValidator();
    }
}