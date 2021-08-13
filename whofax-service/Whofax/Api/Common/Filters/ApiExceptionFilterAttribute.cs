using Microsoft.AspNetCore.Mvc.Filters;
using Whofax.Application.Common.Exceptions;
using Whofax.Api.Common.Helpers;

namespace Whofax.Api.Common.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilterAttribute()
    {
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(ValidationException), HandleValidationException },
            { typeof(NotFoundException), HandleNotFoundException },
            { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
            { typeof(ForbiddenAccessException), HandleForbiddenAccessException },
        };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        var type = context.Exception.GetType();
        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);
            return;
        }

        if (!context.ModelState.IsValid)
        {
            HandleInvalidModelStateException(context);
            return;
        }

        HandleUnknownException(context);
    }

    private void HandleValidationException(ExceptionContext context)
    {
        var exception = (ValidationException) context.Exception;

        context.Result = HttpResponseHelper.CreateBadRequestResponse(exception.Errors);
        context.ExceptionHandled = true;
    }

    private void HandleInvalidModelStateException(ExceptionContext context)
    {
        context.Result = HttpResponseHelper.CreateBadRequestResponse(context.ModelState);
        context.ExceptionHandled = true;
    }

    private void HandleNotFoundException(ExceptionContext context)
    {
        var exception = (NotFoundException) context.Exception;

        context.Result = HttpResponseHelper.CreateNotFoundResponse(exception.Message);
        context.ExceptionHandled = true;
    }

    private void HandleUnauthorizedAccessException(ExceptionContext context)
    {
        context.Result = HttpResponseHelper.CreateUnauthorizedResponse();
        context.ExceptionHandled = true;
    }

    private void HandleForbiddenAccessException(ExceptionContext context)
    {
        context.Result = HttpResponseHelper.CreateForbiddenResponse();
        context.ExceptionHandled = true;
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        context.Result = HttpResponseHelper.CreateInternalServerErrorResponse();
        context.ExceptionHandled = true;
    }
}