namespace JoBoard.AuthService.Domain.Common.Services;

public interface IPasswordStrengthValidator
{
    int MinPasswordLength { get; }
    int MaxPasswordLength { get; }
    bool Validate(string password);
}