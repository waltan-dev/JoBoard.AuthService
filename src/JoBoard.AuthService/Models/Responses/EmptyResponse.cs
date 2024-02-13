namespace JoBoard.AuthService.Models.Responses;

public class EmptyResponse : BaseResponse
{
    public EmptyResponse()
    {
        Code = 200;
        Message = "OK";
    }
}