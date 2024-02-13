namespace JoBoard.AuthService.Models.Responses;

public class BaseDataResponse<TData> : BaseResponse
{
    public TData Data { get; init; }
}