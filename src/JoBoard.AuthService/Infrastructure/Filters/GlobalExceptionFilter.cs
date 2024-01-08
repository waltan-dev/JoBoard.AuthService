using JoBoard.AuthService.Domain.Exceptions;
using JoBoard.AuthService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JoBoard.AuthService.Infrastructure.Filters;

internal class GlobalExceptionFilter : ExceptionFilterAttribute
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }
    
    public override void OnException(ExceptionContext context)
    {
        if (context.Exception is DomainException or ArgumentException)
        {
            _logger.LogWarning(context.Exception, context.Exception.Message);
            
            var resultObject = new ConflictResponse
            {
                Message = context.Exception.Message
            };
            context.Result = new JsonResult(resultObject)
            {
                StatusCode = StatusCodes.Status409Conflict
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