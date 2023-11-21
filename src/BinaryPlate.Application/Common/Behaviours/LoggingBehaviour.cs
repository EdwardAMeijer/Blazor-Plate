namespace BinaryPlate.Application.Common.Behaviours;

/// <summary>
/// Represents a pipeline behavior for logging requests and responses in a request pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class LoggingBehaviour<TRequest, TResponse>(ILogger<TRequest> logger, IHttpContextAccessor httpContextAccessor, IUtcDateTimeProvider dateTimeProvider) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    #region Private Fields

    private readonly ILogger _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    #endregion Private Fields

    #region Public Methods

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Get the name of the request type.
        var requestName = typeof(TRequest).Name;

        // Get the name of the response type.
        var responseName = typeof(TResponse).Name;

        // Get the ID of the user making the request.
        var userId = httpContextAccessor.GetUserId();

        // Call the next handler in the pipeline and get its response.
        var response = await next();

        // Log information about the request
        _logger.LogInformation("Request Info: Request name: {@RequestName} | Request path: {requestPath} | Response name: {ResponseName} | Requested by: {@UserId} | Requested on: {@RequestOn}",
                               requestName,
                               request,
                               responseName,
                               userId,
                               dateTimeProvider.GetUtcNow());

        // Return the response.
        return response;
    }

    #endregion Public Methods
}