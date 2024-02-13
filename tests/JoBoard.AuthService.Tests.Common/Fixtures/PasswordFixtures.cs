using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Common.Services;
using JoBoard.AuthService.Infrastructure.Authentication;

namespace JoBoard.AuthService.Tests.Common.Fixtures;

public static class PasswordFixtures
{
    public static string DefaultPassword = "ValidPassword123#";
    //public static string DefaultPasswordHash = "10000.sV/vif/8IYZO52XF9Tfn5w==.Mi4Hj8hZxlmtkqNlpNRDe1rxrA/Z+6szDZLwQ9vjBrs=";
    public static Password CreateDefault() 
        => Password.Create(DefaultPassword, GetPasswordStrengthValidatorStub(), GetPasswordHasherStub());
    
    public static string NewPassword = "ValidPassword123#";
    //public static string NewPasswordHash = "10000.sV/vif/8IYZO52XF9Tfn5w==.Mi4Hj8hZxlmtkqNlpNRDe1rxrA/Z+6szDZLwQ9vjBrs=";
    public static Password CreateNew() 
        => Password.Create(NewPassword, GetPasswordStrengthValidatorStub(), GetPasswordHasherStub());
    
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

    private static IPasswordStrengthValidator GetPasswordStrengthValidatorStub()
    {
        return new PasswordStrengthValidator();
    }
}