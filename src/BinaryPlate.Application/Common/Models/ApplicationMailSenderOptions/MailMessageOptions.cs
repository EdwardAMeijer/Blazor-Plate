namespace BinaryPlate.Application.Common.Models.ApplicationMailSenderOptions;

/// <summary>
/// Represents options for configuring the content and formatting of email messages.
/// </summary>
public class MailMessageOptions
{
    #region Public Properties

    /// <summary>
    /// Gets the name of the configuration section where these mail message options can be found.
    /// </summary>
    public const string Section = "MailMessageOptions";

    /// <summary>
    /// Gets or sets the sender's email address.
    /// </summary>
    public string From { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the email message body is in HTML format.
    /// </summary>
    public bool IsBodyHtml { get; set; }

    #endregion Public Properties
}