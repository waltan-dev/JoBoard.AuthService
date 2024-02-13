namespace JoBoard.AuthService.Domain.Common.Services;

public interface ISecureTokenizer
{
    public string Generate();
}