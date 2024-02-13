using JoBoard.AuthService.Domain.Aggregates.UserAggregate.ValueObjects;
using JoBoard.AuthService.Domain.Common.Exceptions;

namespace JoBoard.AuthService.Tests.Unit.Tests.Domain.Aggregates.User.ValueObjects;

public class EmailTests
{
    [Theory]
    [MemberData(nameof(GetValidEmails))]
    public void CreateValidEmail(string emailValue)
    {
        var email = new Email(emailValue);
        
        Assert.Equal(emailValue.ToLower(), email.Value);
    }
    
    [Theory]
    [MemberData(nameof(GetInvalidEmails))]
    public void CreateInvalidEmail(string emailValue)
    {
        Assert.Throws<DomainException>(() =>
        {
            _ = new Email(emailValue);
        });
    }

    [Fact]
    public void CreateEmptyEmail()
    {
        Assert.Throws<DomainException>(() =>
        {
            _ = new Email(string.Empty);
        });
    }
    
    private static IEnumerable<object[]> GetValidEmails()
    {
        return new List<object[]>()
        {
            new object[] { "рус@рус.рф" },
            new object[] { "name@sub.domain.com" },
            new object[] { "VeryLongName.ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo@domain.com" },
            new object[] { "name@VeryLongDomainPart.oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo.com" },
            new object[] { "name123@domain123.com" },
        };
    }
    
    private static IEnumerable<object[]> GetInvalidEmails()
    {
        return new List<object[]>()
        {
            new object[] { "VeryLongName.oooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo@domain.com" },
            new object[] { "name@VeryLongDomainPart.ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo.com" },
            new object[] { "code.maze.com" },
            new object[] { "code@maze@codemaze.com" },
            new object[] { "code@.maze.com" },
            new object[] { "name@domain" },
            new object[] { "name@domain." },
            new object[] { "name@-domain.com" },
            new object[] { "name@_domain.com" },
            new object[] { "name@domain-.com" },
            new object[] { "name@domain.com_" },
            new object[] { "name@.domain.com" },
            new object[] { "name@domain..com" },
            new object[] { "name@domain@domain.com" },
            new object[] { "name @ domain.com" },
            new object[] { "“”name””@domain.com" },
            new object[] { "“name”@domain@com" },
            new object[] { "name@123" },
            new object[] { "namedomain.com" },
            new object[] { "name@domain@com" },
            new object[] { "name@domaincom" },
        };
    }
}