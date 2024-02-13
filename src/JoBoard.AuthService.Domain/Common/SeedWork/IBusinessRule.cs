namespace JoBoard.AuthService.Domain.Common.SeedWork;

public interface IBusinessRule
{
    bool IsBroken();
    string Message { get; }
}