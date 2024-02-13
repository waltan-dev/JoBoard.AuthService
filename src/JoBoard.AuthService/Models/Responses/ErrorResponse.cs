namespace JoBoard.AuthService.Models.Responses;

public class ErrorResponse
{
    public string ExceptionType { get; set; }
    public string Message { get; set; }
    public string StackTrace { get; set; }
}