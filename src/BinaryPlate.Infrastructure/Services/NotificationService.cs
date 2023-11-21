namespace BinaryPlate.Infrastructure.Services;

public class NotificationService(IAppOptionsService appOptionsService) : INotificationService
{
    #region Public Methods

    public Task SendSmsAsync(Message message)
    {
        return Task.CompletedTask;
    }

    public Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
    {
        // Get SMTP and mail message options from the AppOptionsService
        var smtpOptions = appOptionsService.GetAppMailSenderOptions().SmtpClientOptions;
        var mailMessageOptions = appOptionsService.GetAppMailSenderOptions().MailMessageOptions;

        // Create MailAddress for sender and recipient
        var from = new MailAddress(mailMessageOptions.From);
        var to = new MailAddress(toEmail);

        // Create a new MailMessage
        using (var message = new MailMessage(from, to))
        {
            // Set email subject and body
            message.Subject = subject;
            message.Body = htmlMessage;
            message.IsBodyHtml = mailMessageOptions.IsBodyHtml;

            // Create an SMTP client
            using var client = new SmtpClient();

            client.Host = smtpOptions.Host;
            client.Port = smtpOptions.Port;
            client.EnableSsl = smtpOptions.EnableSsl;

            // If not using default credentials, set custom credentials
            if (!smtpOptions.UseDefaultCredentials)
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(smtpOptions.CredentialsOptions.UserName, smtpOptions.CredentialsOptions.Password, smtpOptions.CredentialsOptions.Domain);
            }

            // Configure SMTP client settings
            client.Timeout = smtpOptions.Timeout;
            client.DeliveryMethod = (SmtpDeliveryMethod)Enum.Parse(typeof(SmtpDeliveryMethod), smtpOptions.DeliveryMethod);
            client.DeliveryFormat = (SmtpDeliveryFormat)Enum.Parse(typeof(SmtpDeliveryFormat), smtpOptions.DeliveryFormat);

            // Send the email using the configured SMTP client
            //TODO: Uncomment client.Send(message) when the system is in production.
            //client.Send(message);
        }

        return Task.CompletedTask; // Return a completed Task to satisfy the asynchronous method signature
    }

    #endregion Public Methods
}