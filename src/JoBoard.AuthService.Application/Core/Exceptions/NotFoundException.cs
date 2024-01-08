namespace JoBoard.AuthService.Application.Core.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string msg) : base(msg)
    {
        
    }
}