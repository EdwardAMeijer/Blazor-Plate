namespace BinaryPlate.Application.Common.Contracts.Infrastructure;

/// <summary>
/// Provides access to the global configuration settings for the application stored in appsettings.json.
/// </summary>
public interface IAppOptionsService
{
    #region Public Methods

    /// <summary>
    /// Retrieves the options related to the client application.
    /// </summary>
    AppClientOptions GetAppClientOptions();

    /// <summary>
    /// Retrieves the options related to JSON Web Tokens (JWT).
    /// </summary>
    AppJwtOptions GetAppJwtOptions();

    /// <summary>
    /// Retrieves the options related to the Simple Mail Transfer Protocol (SMTP) configuration.
    /// </summary>
    AppMailSenderOptions GetAppMailSenderOptions();

    /// <summary>
    /// Retrieves the options related to the application's user settings.
    /// </summary>
    AppUserOptions GetAppUserOptions();

    /// <summary>
    /// Retrieves the options related to user account lockout policies.
    /// </summary>
    AppLockoutOptions GetAppLockoutOptions();

    /// <summary>
    /// Retrieves the options related to user account password policies.
    /// </summary>
    AppPasswordOptions GetAppPasswordOptions();

    /// <summary>
    /// Retrieves the options related to user account sign-in settings.
    /// </summary>
    AppSignInOptions GetAppSignInOptions();

    /// <summary>
    /// Retrieves the options related to user access tokens and refresh tokens.
    /// </summary>
    AppTokenOptions GetAppTokenOptions();

    /// <summary>
    /// Retrieves the options related to file storage for the application.
    /// </summary>
    AppFileStorageOptions GetAppFileStorageOptions();

    #endregion Public Methods
}
