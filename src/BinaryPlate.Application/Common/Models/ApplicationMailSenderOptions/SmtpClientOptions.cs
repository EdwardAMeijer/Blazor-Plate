namespace BinaryPlate.Application.Common.Models.ApplicationMailSenderOptions;

/// <summary>
/// Represents options for configuring the SMTP client used for sending email messages.
/// </summary>
public class SmtpClientOptions
{
    #region Public Properties

    /// <summary>
    /// Gets the name of the configuration section where these SMTP client options can be found.
    /// </summary>
    public const string Section = "SmtpClientOptions";

    /// <summary>
    /// Gets or sets the options for configuring network credentials.
    /// </summary>
    public CredentialsOptions CredentialsOptions { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to use default network credentials for authentication.
    /// </summary>
    public bool UseDefaultCredentials { get; set; }

    /// <summary>
    /// Gets or sets the port number used for the SMTP server.
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to use SSL/TLS encryption when connecting to the SMTP server.
    /// </summary>
    public bool EnableSsl { get; set; }

    /// <summary>
    /// Gets or sets the timeout, in milliseconds, for SMTP operations.
    /// </summary>
    public int Timeout { get; set; }

    /// <summary>
    /// Gets or sets the delivery method used by the SMTP client.
    /// </summary>
    public string DeliveryMethod { get; set; }

    /// <summary>
    /// Gets or sets the delivery format for the email message.
    /// </summary>
    public string DeliveryFormat { get; set; }

    /// <summary>
    /// Gets or sets the hostname or IP address of the SMTP server.
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// Gets or sets the target name used when establishing a secure connection with the SMTP server.
    /// </summary>
    public string TargetName { get; set; }

    #endregion Public Properties
}