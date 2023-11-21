namespace BinaryPlate.Application.Common.Models.ApplicationMailSenderOptions;

/// <summary>
/// Represents options for configuring network credentials for authentication.
/// </summary>
public class CredentialsOptions
{
    #region Public Properties

    /// <summary>
    /// Gets the name of the configuration section where these SMTP client credentials options can be found.
    /// </summary>
    public const string Section = "CredentialsOptions";

    /// <summary>
    /// Gets or sets the username used for authentication.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the password associated with the username.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the network domain for authentication (optional).
    /// </summary>
    public string Domain { get; set; }

    #endregion Public Properties
}