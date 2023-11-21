namespace BinaryPlate.WebAPI.Services.HubServices;

public class SignalRContextProvider() : ISignalRContextProvider
{
    #region Public Methods

    public string GetHostName(HubCallerContext hubCallerContext)
    {
        // Check if the given HubCallerContext is null, throw an ArgumentNullException if it is.
        ThrowExceptionIfNull(hubCallerContext);

        // Get the HttpContext from the given HubCallerContext and retrieve the request object from it.
        var httpContext = hubCallerContext.GetHttpContext()?.Request;

        // Return a formatted string with the Scheme and Host of the request object.
        return $"{httpContext?.Scheme}://{httpContext?.Host}";
    }

    public string GetUserName(HubCallerContext hubCallerContext)
    {
        // Check if the given HubCallerContext is null, throw an ArgumentNullException if it is.
        ThrowExceptionIfNull(hubCallerContext);

        // Check if the user is authenticated, throw an Exception if they are not.
        if (!hubCallerContext.User.IsAuthenticated())
            throw new Exception(Resource.You_are_not_authorized);

        // Split the username by the "@" symbol and return the first part.
        return hubCallerContext.User?.Identity?.Name?.Split("@")[0];
    }

    public string GetUserNameIdentifier(HubCallerContext hubCallerContext)
    {
        // Check if the given HubCallerContext is null, throw an ArgumentNullException if it is.
        ThrowExceptionIfNull(hubCallerContext);

        // Return the value of the first claim whose type matches ClaimTypes.NameIdentifier.
        return hubCallerContext.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    }

    #endregion Public Methods

    #region Private Methods

    private void ThrowExceptionIfNull(HubCallerContext hubCallerContext)
    {
        // Throw an ArgumentNullException if the given HubCallerContext is null.
        if (hubCallerContext is null)
            throw new ArgumentNullException(nameof(hubCallerContext));
    }

    #endregion Private Methods
}