namespace BinaryPlate.Application.Common.Behaviours;

/// <summary>
/// Represents a pipeline behavior for performing validation on the request before it is handled by
/// the pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse> where TResponse : EnvelopeBase
{
    #region Public Methods

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Use the injected validators to validate the request.
        var failures = validators.Select(v => v.Validate(request))
                                  // Flatten the list of errors.
                                  .SelectMany(result => result.Errors)
                                  // Remove any null values.
                                  .Where(f => f != null)
                                  .ToList();

        // If there are any validation failures, throw a FluentValidationException.
        if (failures.Count != 0)
            throw new FluentValidationException(failures);

        // If there are no validation failures, pass the request to the next middleware in the pipeline.
        return await next();
    }

    #endregion Public Methods
}