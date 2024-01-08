namespace JoBoard.AuthService.Models;

public class ErrorResponse
{
    public string ExceptionType { get; set; }
    public string Message { get; set; }
    public string StackTrace { get; set; }
}