namespace BinaryPlate.Application.Common.Models.ApplicationOptions;

/// <summary>
/// Represents options for configuring the client application.
/// </summary>
public class AppClientOptions
{
    #region Public Fields

    /// <summary>
    /// Gets the name of the section in the configuration file where these options can be found.
    /// </summary>
    public const string Section = "AppClientOptions";

    #endregion Public Fields

    #region Public Properties

    /// <summary>
    /// Gets or sets the host name of the application.
    /// </summary>
    public string HostName { get; set; }

    /// <summary>
    /// Gets or sets the URL for confirming email changes.
    /// </summary>
    public string ConfirmEmailChangeUrl { get; set; }

    /// <summary>
    /// Gets or sets the URL for confirming email addresses.
    /// </summary>
    public string ConfirmEmailUrl { get; set; }

    /// <summary>
    /// Gets or sets the URL for confirming email addresses with a return URL.
    /// </summary>
    public string ConfirmEmailUrlWithReturnUrl { get; set; }

    /// <summary>
    /// Gets or sets the URL for resetting passwords.
    /// </summary>
    public string ResetPasswordUrl { get; set; }

    #endregion Public Properties
}