﻿namespace BinaryPlate.Application.Common.Behaviours;

/// <summary>
/// Represents a pipeline behavior for measuring the performance of a request pipeline.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class PerformanceBehaviour<TRequest, TResponse>(ILogger<TRequest> logger,
                                                       IHttpContextAccessor httpContextAccessor,
                                                       IUtcDateTimeProvider dateTimeProvider) : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    #region Private Fields

    private readonly Stopwatch _timer = new();

    #endregion Private Fields

    #region Public Methods

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // Start the stopwatch to measure the elapsed time of the request.
        _timer.Start();

        // Call the next handler in the pipeline and get its response.
        var response = await next();

        // Stop the stopwatch.
        _timer.Stop();

        // If the elapsed time is less than or equal to 500ms, return the response.
        if (_timer.ElapsedMilliseconds <= 500)
            return response;

        // Get the name of the request type.
        var requestName = typeof(TRequest).Name;

        // Get the name of the response type.
        var responseName = typeof(TResponse).Name;

        // Get the ID of the user making the request.
        var userId = httpContextAccessor.GetUserId();

        // Log information about the request
        logger.LogWarning("Long Running Request: Request name: {@RequestName} | Request path: {requestPath} | Response name: {ResponseName} | Requested by: {@UserId} | Requested on: {@RequestOn} | Requested duration: {ElapsedMilliseconds}",
                               requestName,
                               request,
                               responseName,
                               userId,
                               dateTimeProvider.GetUtcNow(),
                               _timer.ElapsedMilliseconds);

        // Return the response.
        return response;
    }

    #endregion Public Methods
}