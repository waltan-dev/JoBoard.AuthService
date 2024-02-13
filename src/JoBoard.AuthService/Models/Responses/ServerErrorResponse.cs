namespace JoBoard.AuthService.Models.Responses;

public class ServerErrorResponse : BaseResponse
{
    public string? ExceptionType { get; set; }
    public string? StackTrace { get; set; }
}