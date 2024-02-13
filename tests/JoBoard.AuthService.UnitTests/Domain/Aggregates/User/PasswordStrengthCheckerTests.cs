using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Common.Services;

namespace JoBoard.AuthService.UnitTests.Domain.Aggregates.User;

public class PasswordStrengthCheckerTests
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
        Assert.Equal(expectedResult, PasswordStrengthChecker.Check(password));
    }
}