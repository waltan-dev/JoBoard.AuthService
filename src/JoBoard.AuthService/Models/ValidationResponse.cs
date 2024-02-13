using JoBoard.AuthService.Application.Common.Exceptions;

namespace JoBoard.AuthService.Models;

public class ValidationResponse
{
    public IEnumerable<ValidationError> Errors { get; set; }
}