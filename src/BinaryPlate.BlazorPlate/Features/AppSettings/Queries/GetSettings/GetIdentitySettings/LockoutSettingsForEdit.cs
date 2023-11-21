namespace BinaryPlate.BlazorPlate.Features.AppSettings.Queries.GetSettings.GetIdentitySettings;

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

    public LockoutSettingsModel MapToCommand()
    {
        return new LockoutSettingsModel
        {
            Id = Id,
            AllowedForNewUsers = AllowedForNewUsers,
            MaxFailedAccessAttempts = MaxFailedAccessAttempts,
            DefaultLockoutTimeSpan = DefaultLockoutTimeSpan,
            ConcurrencyStamp = ConcurrencyStamp
        };
    }

    #endregion Public Methods
}