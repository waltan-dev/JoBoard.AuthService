using System.Text.Json;
using JoBoard.AuthService.Application.Exceptions;
using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JoBoard.AuthService.Middlewares.Filters;

internal class GlobalExceptionFilter : ExceptionFilterAttribute
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }
    
    public override void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case ValidationException:
                SetValidationResponse(context);
                break;
            case DomainException:
                SetConflictResponse(context);
                break;
            case NotFoundException:
                SetNotFoundResponse(context);
                break;
            default:
                SetErrorResponse(context);
                break;
        }
    }
    
    private void SetValidationResponse(ExceptionContext context)
    {
        var errors = ((ValidationException)context.Exception).Errors;
        _logger.LogWarning($"{context.Exception.Message}: {JsonSerializer.Serialize(errors)}");
        var resultObject = new ValidationResponse
        {
            Code = StatusCodes.Status422UnprocessableEntity,
            Message = context.Exception.Message,
            Errors = errors
        };
        context.Result = new JsonResult(resultObject)
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity
        };
    }
    
    private void SetConflictResponse(ExceptionContext context)
    {
        _logger.LogWarning(context.Exception.Message);
        var resultObject = new ConflictResponse
        {
            Code = StatusCodes.Status409Conflict,
            Message = context.Exception.Message
        };
        context.Result = new JsonResult(resultObject)
        {
            StatusCode = StatusCodes.Status409Conflict
        };
    }

    private void SetNotFoundResponse(ExceptionContext context)
    {
        _logger.LogWarning(context.Exception.Message);
        var resultObject = new NotFoundResponse
        {
            Code = StatusCodes.Status404NotFound,
            Message = context.Exception.Message
        };
        context.Result = new JsonResult(resultObject)
        {
            StatusCode = StatusCodes.Status404NotFound
        };
    }

    private void SetErrorResponse(ExceptionContext context)
    {
        _logger.LogError(context.Exception, context.Exception.Message);
        var ex = context.Exception;
        var resultObject = new ServerErrorResponse
        {
            Code = StatusCodes.Status500InternalServerError,
            ExceptionType = ex.GetType().FullName,
            Message = ex.Message,
            StackTrace = ex.StackTrace
        };
        context.Result = new JsonResult(resultObject)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}