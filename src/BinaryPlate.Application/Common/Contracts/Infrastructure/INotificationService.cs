namespace BinaryPlate.Application.Common.Contracts.Infrastructure;

/// <summary>
/// Represents a service for sending messages via SMS, email, and other means.
/// </summary>
public interface INotificationService
{
    #region Public Methods

    /// <summary>
    /// Sends an SMS message asynchronously.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendSmsAsync(Message message);

    /// <summary>
    /// Sends an email message asynchronously.
    /// </summary>
    /// <param name="toEmail">The recipient's email address.</param>
    /// <param name="subject">The email subject.</param>
    /// <param name="htmlMessage">The HTML body of the email.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendEmailAsync(string toEmail, string subject, string htmlMessage);

    #endregion Public Methods
}