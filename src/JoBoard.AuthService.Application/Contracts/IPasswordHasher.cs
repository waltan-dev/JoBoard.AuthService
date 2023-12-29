namespace JoBoard.AuthService.Application.Contracts;

public interface IPasswordHasher
{
    public string Hash(string password);
    bool Verify(string hash, string password);
}