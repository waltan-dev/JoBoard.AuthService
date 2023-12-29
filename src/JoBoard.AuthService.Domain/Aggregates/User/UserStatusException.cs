namespace JoBoard.AuthService.Domain.Aggregates.User;

public class UserStatusException : Exception
{
    public UserStatusException(string msg) : base(msg)
    {
        
    }
}