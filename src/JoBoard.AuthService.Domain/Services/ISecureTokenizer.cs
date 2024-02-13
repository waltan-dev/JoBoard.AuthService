namespace JoBoard.AuthService.Domain.Services;

public interface ISecureTokenizer
{
    public string Generate();
}