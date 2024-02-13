namespace JoBoard.AuthService.Application.Common.Models;

public class LoginResult
{
    public Guid UserId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Role { get; init; }
    
    public LoginResult(Guid userId, 
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