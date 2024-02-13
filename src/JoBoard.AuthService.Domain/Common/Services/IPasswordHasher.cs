namespace JoBoard.AuthService.Domain.Common.Services;

public interface IPasswordHasher
{
    public string Hash(string password);
    bool Verify(string hash, string password);
}