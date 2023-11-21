namespace BinaryPlate.Application.Common.Models.ApplicationOptions;

/// <summary>
/// Represents a set of configuration options for the application.
/// </summary>
public class AppOptions
{
    #region Public Fields

    /// <summary>
    /// Gets the name of the configuration section where these options can be found.
    /// </summary>
    public const string Section = "AppOptions";

    #endregion Public Fields

    #region Public Properties

    /// <summary>
    /// Gets or sets the client options for the application.
    /// </summary>
    public AppClientOptions AppClientOptions { get; set; }

    /// <summary>
    /// Gets or sets the JWT (JSON Web Token) options for the application.
    /// </summary>
    public AppJwtOptions AppJwtOptions { get; set; }

    /// <summary>
    /// Gets or sets the mail sender options for the application.
    /// </summary>
    public AppMailSenderOptions AppMailSenderOptions { get; set; }

    /// <summary>
    /// Gets or sets the identity options for the application.
    /// </summary>
    public AppIdentityOptions AppIdentityOptions { get; set; }

    /// <summary>
    /// Gets or sets the token options for the application.
    /// </summary>
    public AppTokenOptions AppTokenOptions { get; set; }

    /// <summary>
    /// Gets or sets the file storage options for the application.
    /// </summary>
    public AppFileStorageOptions AppFileStorageOptions { get; set; }

    #endregion Public Properties
}
