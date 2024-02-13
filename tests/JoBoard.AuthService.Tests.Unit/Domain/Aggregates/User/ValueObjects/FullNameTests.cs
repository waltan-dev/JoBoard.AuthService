using JoBoard.AuthService.Domain.Aggregates.User.ValueObjects;

namespace JoBoard.AuthService.Tests.Unit.Domain.Aggregates.User.ValueObjects;

public class FullNameTests
{
    [Fact]
    public void CreateValid()
    {
        var fullName = new FullName("Ivan", "Ivanov");
        
        Assert.Equal("Ivan", fullName.FirstName);
        Assert.Equal("Ivanov", fullName.LastName);
    }
    
    [Fact]
    public void CreateInvalid()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _ = new FullName(" ", string.Empty);
        });
    }
    
    [Fact]
    public void NameToString()
    {
        var fullName = new FullName("Ivan", "Ivanov");
        
        Assert.Equal("Ivan Ivanov", fullName.ToString());
    }
    
    [Fact]
    public void Compare()
    {
        var fullName1 = new FullName("Ivan", "Ivanov");
        var fullName2 = new FullName("Ivan", "Ivanov");
        
        Assert.Equal(fullName1, fullName2);
    }
}