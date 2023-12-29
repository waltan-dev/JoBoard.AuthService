using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.Domain.UnitTests.Aggregates.User;

public class EmailTest
{
    [Fact]
    public void CreateValidEmail()
    {
        var email = new Email("TeSt@GmAiL.cOm");
        
        Assert.Equal("test@gmail.com", email.Value);
    }

    [Fact]
    public void CreateInvalidEmail()
    {
        Assert.Throws<InvalidEmailException>(() =>
        {
            _ = new Email("invalid-email");
        });
    }

    [Fact]
    public void CreateEmptyEmail()
    {
        Assert.Throws<InvalidEmailException>(() =>
        {
            _ = new Email(string.Empty);
        });
    }
}