namespace JoBoard.AuthService.Application.Models;

public class LoginResult
{
    public string UserId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Role { get; init; }
    
    public LoginResult(string userId, 
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