using JoBoard.AuthService.Domain.Aggregates.User;
using JoBoard.AuthService.Domain.Exceptions;

namespace JoBoard.AuthService.UnitTests.Domain.Aggregates.User;

public class DeactivateUserTests
{
    [Fact]
    public void Deactivate()
    {
        var user = new UserBuilder().WithActiveStatus().Build();
        
        user.Deactivate();
        
        Assert.Equal(UserStatus.Deactivated, user.Status);
    }
    
    [Fact]
    public void DeactivateWithInactiveStatus()
    {
        var user = new UserBuilder().Build();

        Assert.Throws<DomainException>(() =>
        {
            user.Deactivate();
        });
    }
}