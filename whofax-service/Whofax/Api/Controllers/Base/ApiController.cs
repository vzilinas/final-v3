using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Whofax.Api.Common.Helpers;

public abstract class ApiController : ControllerBase
{
    /// <summary>
    /// Creates an <see cref="BadRequestObjectResult"/> that produces a <see cref="StatusCodes.Status400BadRequest"/> response.
    /// </summary>
    /// <param name="modelState">The <see cref="ModelStateDictionary" /> containing errors to be returned to the client.</param>
    /// <returns>The created <see cref="BadRequestObjectResult"/> for the response.</returns>
    public override BadRequestObjectResult BadRequest(ModelStateDictionary modelState)
    {
        return HttpResponseHelper.CreateBadRequestResponse(modelState);
    }

    /// <summary>
    /// Creates an <see cref="NotFoundObjectResult"/> that produces a <see cref="StatusCodes.Status404NotFound"/> response.
    /// </summary>
    /// <returns>The created <see cref="NotFoundObjectResult"/> for the response.</returns>
    public override NotFoundObjectResult NotFound(object value)
    {
        return HttpResponseHelper.CreateNotFoundResponse(value);
    }
}
