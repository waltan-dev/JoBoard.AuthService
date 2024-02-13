using JoBoard.AuthService.Infrastructure.Common.Services;

namespace JoBoard.AuthService.Tests.Unit.Tests.Infrastructure.Authentication;

public class PasswordStrengthValidatorTests
{
    // min 6 chars
    // max 32 chars
    // at least 1 uppercase, 1 lowercase, 1 digit, 1 non-alphanumeric
    
    [Theory]
    [InlineData("AAbb123$", true)]
    [InlineData("", false)]
    [InlineData("12345", false)]
    [InlineData("qwert", false)]
    [InlineData("Qwerty", false)]
    [InlineData("Qwerty123456", false)]
    [InlineData("Qwertyuiop123456789_Qwertyuiop123456789_Qwertyuiop123456789", false)]
    public void CheckValid(string password, bool expectedResult)
    {
        var passwordStrengthValidator = new PasswordStrengthValidator();
        Assert.Equal(expectedResult, passwordStrengthValidator.Validate(password));
    }
}