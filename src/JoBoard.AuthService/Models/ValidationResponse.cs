using JoBoard.AuthService.Application.Core.Exceptions;

namespace JoBoard.AuthService.Models;

public class ValidationResponse
{
    public IEnumerable<ValidationError> Errors { get; set; }
}