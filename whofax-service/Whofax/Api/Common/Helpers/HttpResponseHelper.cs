using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Whofax.Api.Common.Helpers;

public static class HttpResponseHelper
{
    public static BadRequestObjectResult CreateBadRequestResponse(ModelStateDictionary modelState)
    {
        var details = new ValidationProblemDetails(modelState)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        return new BadRequestObjectResult(details);
    }

    public static BadRequestObjectResult CreateBadRequestResponse(IDictionary<string, string[]> errors)
    {
        var details = new ValidationProblemDetails(errors)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        return new BadRequestObjectResult(details);
    }

    public static NotFoundObjectResult CreateNotFoundResponse(object detail)
    {
        var details = new ProblemDetails()
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
            Detail = detail as string
        };

        return new NotFoundObjectResult(details);
    }

    public static UnauthorizedObjectResult CreateUnauthorizedResponse()
    {
        var details = new ProblemDetails
        {
            Title = "Unauthorized",
            Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
        };

        return new UnauthorizedObjectResult(details);
    }

    public static ObjectResult CreateForbiddenResponse()
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Title = "Forbidden",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
        };

        return new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status403Forbidden
        };
    }

    public static ObjectResult CreateInternalServerErrorResponse()
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An error occurred while processing your request.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };

        return new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
}
