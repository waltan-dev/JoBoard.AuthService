using JoBoard.AuthService.Domain.Aggregates.User;

namespace JoBoard.AuthService.Domain.UnitTests.Aggregates.User;

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
}