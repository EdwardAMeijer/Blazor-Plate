namespace BinaryPlate.Application.Common.Models.ApplicationMailSenderOptions;

/// <summary>
/// Represents options for configuring email sending via SMTP.
/// </summary>
public class AppMailSenderOptions
{
    #region Public Fields

    /// <summary>
    /// Gets the name of the configuration section where these email sender options can be found.
    /// </summary>
    public const string Section = "AppMailSenderOptions";

    #endregion Public Fields

    #region Public Properties

    /// <summary>
    /// Gets or sets the options for configuring the content of email messages.
    /// </summary>
    public MailMessageOptions MailMessageOptions { get; set; }

    /// <summary>
    /// Gets or sets the options for configuring the SMTP client used for sending emails.
    /// </summary>
    public SmtpClientOptions SmtpClientOptions { get; set; }

    #endregion Public Properties
}