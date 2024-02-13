using JoBoard.AuthService.Infrastructure.Authentication;

namespace JoBoard.AuthService.UnitTests.Infrastructure.Authentication;

public class PasswordStrengthValidatorTests
{
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