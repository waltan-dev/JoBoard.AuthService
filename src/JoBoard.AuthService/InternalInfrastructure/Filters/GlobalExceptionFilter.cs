using System.Text.Json;
using JoBoard.AuthService.Application.Common.Exceptions;
using JoBoard.AuthService.Domain.Common.Exceptions;
using JoBoard.AuthService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JoBoard.AuthService.InternalInfrastructure.Filters;

internal class GlobalExceptionFilter : ExceptionFilterAttribute
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }
    
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is ValidationException validationException)
        {
            _logger.LogWarning($"{context.Exception.Message}: {JsonSerializer.Serialize(validationException.Errors)}");
            
            var resultObject = new ValidationResponse { Errors = validationException.Errors };
            context.Result = new JsonResult(resultObject)
            {
                StatusCode = StatusCodes.Status422UnprocessableEntity
            };
        }
        else if (context.Exception is DomainException or ArgumentException or NotFoundException)
        {
            // TODO implement Guards with DomainException and remove ArgumentException from here
            _logger.LogWarning(context.Exception.Message);
            
            var resultObject = new ConflictResponse { Message = context.Exception.Message };
            context.Result = new JsonResult(resultObject)
            {
                StatusCode = StatusCodes.Status409Conflict
            };
        }
        else if (context.Exception is NotFoundException)
        {
            _logger.LogWarning(context.Exception.Message);
            
            var resultObject = new ConflictResponse { Message = context.Exception.Message };
            context.Result = new JsonResult(resultObject)
            {
                StatusCode = StatusCodes.Status404NotFound
            };
        }
        else
        {
            _logger.LogError(context.Exception, context.Exception.Message);
            
            var resultObject = new ErrorResponse
            {
                ExceptionType = context.Exception.GetType().FullName,
                Message = context.Exception.Message,
                StackTrace = context.Exception.StackTrace
            };
            context.Result = new JsonResult(resultObject)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}