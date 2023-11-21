namespace BinaryPlate.Application.Features.AppSettings.Queries.GetSettings.GetIdentitySettings;

public class LockoutSettingsForEdit
{
    #region Public Properties

    public string Id { get; set; }
    public bool AllowedForNewUsers { get; set; }
    public int MaxFailedAccessAttempts { get; set; }
    public int DefaultLockoutTimeSpan { get; set; }
    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static LockoutSettingsForEdit MapFromEntity(LockoutSettings lockoutSettings)
    {
        return new LockoutSettingsForEdit
        {
            Id = lockoutSettings.Id.ToString(),
            AllowedForNewUsers = lockoutSettings.AllowedForNewUsers,
            MaxFailedAccessAttempts = lockoutSettings.MaxFailedAccessAttempts,
            DefaultLockoutTimeSpan = lockoutSettings.DefaultLockoutTimeSpan,
            ConcurrencyStamp = lockoutSettings.ConcurrencyStamp
        };
    }

    #endregion Public Methods
}