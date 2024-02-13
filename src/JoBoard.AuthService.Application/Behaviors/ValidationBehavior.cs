using FluentValidation;
using JoBoard.AuthService.Application.Exceptions;
using MediatR;

namespace JoBoard.AuthService.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationTasks = _validators.Select(validator => validator.ValidateAsync(context, ct));
        var validationResults = await Task.WhenAll(validationTasks);
        var errors = validationResults
            .Where(validationResult => validationResult.IsValid == false)
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure => new ValidationError(
                validationFailure.PropertyName,
                validationFailure.ErrorMessage))
            .ToList();
        
        if (errors.Any())
            throw new Exceptions.ValidationException(errors);

        return await next();
    }
}