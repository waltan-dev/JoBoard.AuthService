namespace JoBoard.AuthService.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public List<ValidationError> Errors { get; }

    public ValidationException(List<ValidationError> errors) 
        : base("One or more validation errors occurred")
    {
        Errors = errors;
    }
    
    public ValidationException(string propertyName, string message) 
        : base("One or more validation errors occurred")
    {
        Errors = new List<ValidationError>() { new(propertyName, message) };
    }
}

public class ValidationError
{
    public string PropertyName { get; }
    public string Message { get; }

    public ValidationError(string propertyName, string message)
    {
        PropertyName = propertyName;
        Message = message;
    }
}