namespace JoBoard.AuthService.Application.Models;

public class UserResult
{
    public Guid UserId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Email { get; }
    public string Role { get; }

    public UserResult(Guid userId, 
        string firstName, string lastName, 
        string email, 
        string role)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Role = role;
    }
}