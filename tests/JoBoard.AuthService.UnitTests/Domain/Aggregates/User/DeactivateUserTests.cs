using JoBoard.AuthService.Domain.Aggregates.User;

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
}