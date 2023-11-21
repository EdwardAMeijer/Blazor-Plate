using Microsoft.Extensions.DependencyInjection;

namespace BinaryPlate.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    #region Public Methods

    /// <summary>
    /// Adds application settings options to the specified <see cref="IServiceCollection"/> instance.
    /// </summary>
    /// <param name="services">The service collection to add the options to.</param>
    /// <param name="configuration">
    /// The <see cref="IConfiguration"/> instance to retrieve the settings from.
    /// </param>
    /// <returns>The modified <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppOptions>(configuration.GetSection(AppOptions.Section));
        services.Configure<AppJwtOptions>(configuration.GetSection(AppJwtOptions.Section));
        services.Configure<AppMailSenderOptions>(configuration.GetSection(AppMailSenderOptions.Section));
        services.Configure<AppClientOptions>(configuration.GetSection(AppClientOptions.Section));
        return services;
    }

    /// <summary>
    /// Adds application localization services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddAppLocalization(this IServiceCollection services)
    {
        // Configure localization options.
        services.AddLocalization(options => options.ResourcesPath = "Resource");

        // Add the localization service implementation.
        services.AddSingleton<ILocalizationService, LocalizationService>();

        return services;
    }

    /// <summary>
    /// Configures authentication services for the application.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">
    /// The <see cref="IConfiguration"/> instance containing the JWT configuration.
    /// </param>
    /// <returns>The <see cref="IServiceCollection"/> instance.</returns>
    public static AuthenticationBuilder AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure authentication options.
        var appJwtOptions = services.BuildServiceProvider().GetService<IAppOptionsService>().GetAppJwtOptions();
        var builder = services.AddAuthentication(GetAuthenticationOptions)
                              .AddCookie(GetCookieAuthenticationOptions)
                              .AddJwtBearer(appJwtOptions)
                              .AddMicrosoft(configuration)
                              .AddGoogle(configuration)
                              .AddLinkedIn(configuration)
                              .AddTwitter(configuration);
        return builder;
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Gets the authentication options with default JWT Bearer authentication scheme.
    /// </summary>
    /// <param name="options">The <see cref="AuthenticationOptions"/> instance to configure.</param>
    private static void GetAuthenticationOptions(AuthenticationOptions options)
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }

    /// <summary>
    /// Gets the cookie authentication options with the specified cookie name and SameSite policy.
    /// </summary>
    /// <param name="options">The <see cref="CookieAuthenticationOptions"/> instance to configure.</param>
    private static void GetCookieAuthenticationOptions(CookieAuthenticationOptions options)
    {
        options.Cookie.Name = "BpCookie";
        options.Cookie.SameSite = SameSiteMode.None; // Required for cross-origin cookies.
    }

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