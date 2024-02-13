namespace JoBoard.AuthService.Application.Common.Models;

public class UserResult
{
    public Guid UserId { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Role { get; init; }

    protected UserResult() { }
    
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