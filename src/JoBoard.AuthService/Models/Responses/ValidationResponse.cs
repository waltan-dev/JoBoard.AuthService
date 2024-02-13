using JoBoard.AuthService.Application.Common.Exceptions;

namespace JoBoard.AuthService.Models.Responses;

public class ValidationResponse : BaseResponse
{
    public IEnumerable<ValidationError> Errors { get; set; }
}