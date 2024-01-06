using JoBoard.AuthService.Infrastructure.Authentication;

namespace JoBoard.AuthService.UnitTests.Infrastructure.Authentication;

public class PasswordHasherTests
{
    [Fact]
    public void Hash()
    {
        var passwordHasher = new PasswordHasher();
        var password = "password";
        
        var hash = passwordHasher.Hash(password);
        
        Assert.NotEmpty(hash);
        Assert.Equal(3, hash.Split('.', 3).Length);
    }
    
    [Fact]
    public void HashEmpty()
    {
        var passwordHasher = new PasswordHasher();
        Assert.Throws<ArgumentException>(() =>
        {
            _ = passwordHasher.Hash(" ");
        });
    }
    
    [Fact]
    public void VerifyValid()
    {
        var passwordHasher = new PasswordHasher();
        var password = "password";
        var hash = passwordHasher.Hash(password);

        var isValid = passwordHasher.Verify(hash, password);
        
        Assert.True(isValid);
    }
    
    [Fact]
    public void VerifyInvalid()
    {
        var passwordHasher = new PasswordHasher();
        var hash = passwordHasher.Hash("password111");

        var isValid = passwordHasher.Verify(hash, "password222");
        
        Assert.False(isValid);
    }
}