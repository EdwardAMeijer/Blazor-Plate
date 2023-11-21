using Microsoft.AspNetCore.Routing;

namespace BinaryPlate.Application.Common.Extensions;

/// <summary>
/// Provides extension methods for accessing properties of the <see cref="HttpContext"/> class.
/// </summary>
public static class HttpContextExtensions
{
    #region Public Methods

    /// <summary>
    /// Gets the user ID from the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> instance.</param>
    /// <returns>The user ID as a string, or an empty string if it is not found.</returns>
    public static string GetUserId(this IHttpContextAccessor httpContextAccessor)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        return userId ?? string.Empty;
    }

    /// <summary>
    /// Gets the user name from the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> instance.</param>
    /// <returns>The user name as a string.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the <see cref="UserManager{TUser}"/> instance is null.
    /// </exception>
    public static string GetUserName(this IHttpContextAccessor httpContextAccessor)
    {
        var userManager = httpContextAccessor.HttpContext?.RequestServices.GetService<UserManager<ApplicationUser>>();

        if (userManager == null)
            throw new ArgumentException(nameof(userManager));

        var userName = userManager.GetUserName(httpContextAccessor.HttpContext.User);

        return userName;
    }

    /// <summary>
    /// Gets the language from the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> instance.</param>
    /// <returns>The language as a string.</returns>
    public static string GetLanguage(this IHttpContextAccessor httpContextAccessor)
    {
        var language = httpContextAccessor.HttpContext?.Request.Headers["Accept-Language"].ToString();

        return language;
    }

    /// <summary>
    /// Extension method to get the name of the requested controller from the provided
    /// IHttpContextAccessor object.
    /// </summary>
    /// <param name="httpContextAccessor">The IHttpContextAccessor object.</param>
    /// <returns>The name of the requested controller.</returns>
    public static string GetControllerName(this IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext == null)
            return string.Empty;

        var controllerName = httpContextAccessor.HttpContext.GetRouteData().Values["controller"]?.ToString();
        return controllerName;
    }


    /// <summary>
    /// Gets the client application host name from the <see cref="HttpContext"/>.
    /// </summary>
    /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/> instance.</param>
    /// <returns>The client application host name as a string.</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string GetClientAppHostName(this IHttpContextAccessor httpContextAccessor)
    {
        // Check if the HttpContext is available
        if (httpContextAccessor.HttpContext == null)
            throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));

        // Get the application options service from the request services
        var appOptionsService = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IAppOptionsService>();

        // Retrieve client application options
        var clientAppOptions = appOptionsService.GetAppClientOptions();

        return clientAppOptions.HostName;
    }

    #endregion Public Methods
}