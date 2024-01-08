using JoBoard.AuthService.Application.Exceptions;

namespace JoBoard.AuthService.Models;

public class ValidationResponse
{
    public IEnumerable<ValidationError> Errors { get; set; }
}