namespace BinaryPlate.Domain.Entities.Settings.IdentitySettings;

/// <summary>
/// Represents the lockout settings for user accounts.
/// </summary>
public class LockoutSettings : ISettingsSchema, IConcurrencyStamp
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the unique identifier for the lockout settings.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether lockout is allowed for new users.
    /// </summary>
    public bool AllowedForNewUsers { get; set; }

    /// <summary>
    /// Gets or sets the maximum number of failed access attempts allowed before a user is locked out.
    /// </summary>
    public int MaxFailedAccessAttempts { get; set; }

    /// <summary>
    /// Gets or sets the default lockout duration in minutes.
    /// </summary>
    public int DefaultLockoutTimeSpan { get; set; }

    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties
}