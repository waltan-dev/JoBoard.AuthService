namespace JoBoard.AuthService.Models.Requests;

public class LoginByPasswordRequest
{
    public string Email { get; init; }
    public string Password { get; init; }
}