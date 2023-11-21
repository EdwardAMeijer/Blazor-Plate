namespace BinaryPlate.BlazorPlate.Features.AppSettings.Commands.UpdateSettings.UpdateIdentitySettings.IdentitySettingsCommand;

public class LockoutSettingsModel
{
    #region Public Properties

    public string Id { get; set; }
    public bool AllowedForNewUsers { get; set; }
    public int MaxFailedAccessAttempts { get; set; }
    public int DefaultLockoutTimeSpan { get; set; }
    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties
}