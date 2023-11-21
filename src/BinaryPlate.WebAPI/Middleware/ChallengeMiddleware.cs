namespace BinaryPlate.WebAPI.Middleware;

/// <summary>
/// This middleware enforce access control and intercepts unauthorized requests.
/// </summary>
public class ChallengeMiddleware(RequestDelegate requestDelegate)
{
    #region Private Fields

    private readonly RequestDelegate _request = requestDelegate ?? throw new ArgumentNullException(nameof(requestDelegate));

    #endregion Private Fields

    #region Public Methods

    public async Task InvokeAsync(HttpContext context)
    {
        // If the context is null, throw an ArgumentNullException with a descriptive message.
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        // Invoke the request and await the result.
        await _request(context);

        // Check the HTTP response status code.
        switch (context.Response.StatusCode)
        {
            // If the status code is 401 (Unauthorized), throw an ApiProblemDetailsException with a
            // message indicating the user is not authorized.
            case 401:
                throw new UnauthorizedAccessException(string.Format(Resource.You_are_not_authorized, context.Request.GetDisplayUrl().Split('?')[0]));

            // If the status code is 403 (Forbidden), throw an ApiProblemDetailsException with a
            // message indicating the user is forbidden.
            case 403:
                throw new ForbiddenAccessException(string.Format(Resource.You_are_forbidden, context.Request.PathBase));
        }
    }

    #endregion Public Methods
}