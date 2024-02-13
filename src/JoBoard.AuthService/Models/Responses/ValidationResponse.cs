using JoBoard.AuthService.Application.Exceptions;

namespace JoBoard.AuthService.Models.Responses;

public class ValidationResponse : BaseResponse
{
    public IEnumerable<ValidationError> Errors { get; set; }
}