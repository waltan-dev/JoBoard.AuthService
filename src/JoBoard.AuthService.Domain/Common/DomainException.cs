namespace JoBoard.AuthService.Domain.Common;

public class DomainException : Exception
{
    public DomainException(string msg) : base(msg)
    {
        
    }
}