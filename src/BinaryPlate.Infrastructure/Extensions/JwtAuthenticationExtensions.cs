namespace BinaryPlate.Infrastructure.Extensions;

/// <summary>
/// Extensions for configuring JWT authentication.
/// </summary>
public static class JwtAuthenticationExtensions
{
    #region Public Methods

    /// <summary>
    /// Adds JWT Bearer authentication to the <see cref="AuthenticationBuilder"/> using the provided
    /// JWT settings.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="AuthenticationBuilder"/> to add the JWT Bearer authentication to.
    /// </param>
    /// <param name="appJwtSettings">The JWT settings.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/> instance.</returns>
    public static AuthenticationBuilder AddJwtBearer(this AuthenticationBuilder builder, AppJwtOptions appJwtSettings)
    {
        // Configure JWT options for DI.
        builder.Services.Configure<AppJwtOptions>(options =>
        {
            options.Issuer = appJwtSettings.Issuer;
            options.Audience = appJwtSettings.Audience;
            options.SecurityKey = appJwtSettings.SecurityKey;
        });

        // Configure JWT Bearer authentication.
        builder.AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                LifetimeValidator = LifetimeValidator, // Custom lifetime validator.
                ClockSkew = TimeSpan.Zero,
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                ValidIssuer = appJwtSettings.Issuer,
                ValidAudience = appJwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appJwtSettings.SecurityKey ?? throw new InvalidOperationException($"{nameof(appJwtSettings.SecurityKey)} cannot be Null."))),
            };
            options.Events = new JwtBearerEvents
            {
                // Event to handle message received.
                OnMessageReceived = context =>
                {
                    // Get the access token from the request header.
                    var accessToken = context.Request.Query["access_token"];

                    // Check if the request is coming from SignalR Hub...
                    var isSignalRRequest = context.Request.Headers.ContainsKey("Sec-WebSocket-Extensions");

                    if (!string.IsNullOrEmpty(accessToken) && isSignalRRequest)
                        // Read the token out of the query string.
                        context.Token = accessToken;

                    // Get a completed task.
                    return Task.CompletedTask;
                }
            };
        });

        return builder;
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Custom lifetime validator for JWT tokens.
    /// </summary>
    /// <param name="notBefore">The 'not before' value of the token.</param>
    /// <param name="expires">The 'expires' value of the token.</param>
    /// <param name="tokenToValidate">The token to validate.</param>
    /// <param name="param">The <see cref="TokenValidationParameters"/> instance.</param>
    /// <returns>True if the token is valid, false otherwise.</returns>
    private static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken tokenToValidate, TokenValidationParameters param)
    {
        if (expires != null)
            return expires > DateTime.UtcNow;

        return false;
    }

    #endregion Private Methods
}