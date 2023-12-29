using JoBoard.AuthService.Domain.Common;

namespace JoBoard.AuthService.Domain.Aggregates.User;

public class InvalidEmailException : DomainException
{
    public InvalidEmailException(string msg) : base(msg)
    {
        
    }
}