namespace JoBoard.AuthService.Models.Responses;

public abstract class BaseResponse
{
    public int Code { get; init; }
    public string Message { get; init; }
}