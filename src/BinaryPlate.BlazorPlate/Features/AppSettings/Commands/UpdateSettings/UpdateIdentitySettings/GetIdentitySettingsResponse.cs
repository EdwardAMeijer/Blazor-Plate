namespace BinaryPlate.BlazorPlate.Features.AppSettings.Commands.UpdateSettings.UpdateIdentitySettings;

public class GetIdentitySettingsResponse
{
    #region Public Properties

    public string UserSettingsId { get; set; }
    public string PasswordSettingsId { get; set; }
    public string LockoutSettingsId { get; set; }
    public string SignInSettingsId { get; set; }
    public string SuccessMessage { get; set; }
    public string LockoutSettingsConcurrencyStamp { get; set; }
    public string PasswordSettingsConcurrencyStamp { get; set; }
    public string SignInSettingsConcurrencyStamp { get; set; }
    public string UserSettingsConcurrencyStamp { get; set; }

    #endregion Public Properties
}