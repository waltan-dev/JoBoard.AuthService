using System.ComponentModel;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Domain.SeedWork;

namespace JoBoard.AuthService.Tests.Unit.Tests.Domain.SeedWork;

public class EnumerationTests
{
    [Fact]
    public void ParseInvalid()
    {
        Assert.Throws<DomainException>(() =>
        {
            Enumeration.FromDisplayName<TestEnumeration>("invalid-name");
        });
    }

    [Fact]
    public void EqualsWithNonEnumeration()
    {
        var testEnum = TestEnumeration.TestValue;

        var result = testEnum.Equals(new object());
        
        Assert.False(result);
    }
    
    private class TestEnumeration : Enumeration
    {
        public static TestEnumeration TestValue = new(1, "Test");
        
        protected TestEnumeration(int id, string name) : base(id, name)
        {
        }
    }
}