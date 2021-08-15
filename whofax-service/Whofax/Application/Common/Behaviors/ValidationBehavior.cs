
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ValidationException = Whofax.Application.Common.Exceptions.ValidationException;

namespace Whofax.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest :notnull
{
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(ILogger<ValidationBehavior<TRequest, TResponse>> logger, IEnumerable<IValidator<TRequest>> validators)
    {
        _logger = logger;
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (_validators.Any())
        {
            _logger.LogInformation("Validating request of type '{@RequestType}.'", typeof(TRequest).Name);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(request, cancellationToken)));
            var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            if (failures.Any())
            {
                _logger.LogWarning("Validation for '{@RequestType}' failed: {validationErrors}", typeof(TRequest).Name, string.Join(';', failures.Select(x => x.ErrorMessage)));
                throw new ValidationException(failures);
            }
        }

        var response = await next();
        return response;
    }
}
