namespace BinaryPlate.Application.Common.Models.ApplicationOptions;

/// <summary>
/// Represents options for configuring JSON Web Token (JWT) authentication.
/// </summary>
public class AppJwtOptions
{
    #region Public Fields

    /// <summary>
    /// Gets the name of the configuration section where these JWT options can be found.
    /// </summary>
    public const string Section = "AppJwtOptions";

    #endregion Public Fields

    #region Public Properties

    /// <summary>
    /// Gets or sets the issuer of JWT tokens, which is used when creating JWT tokens.
    /// </summary>
    public string Issuer { get; set; }

    /// <summary>
    /// Gets or sets the security key for JWT tokens, used for both token creation and validation.
    /// </summary>
    public string SecurityKey { get; set; }

    /// <summary>
    /// Gets or sets the audience of JWT tokens, specifying the intended recipients of the tokens.
    /// </summary>
    public string Audience { get; set; }

    #endregion Public Properties
}