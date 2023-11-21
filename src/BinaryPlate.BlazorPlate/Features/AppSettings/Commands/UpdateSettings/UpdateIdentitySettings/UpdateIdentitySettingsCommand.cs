namespace BinaryPlate.BlazorPlate.Features.AppSettings.Commands.UpdateSettings.UpdateIdentitySettings;

public class UpdateIdentitySettingsCommand
{
    #region Public Properties

    public UserSettingsModel UserSettingsModel { get; set; }
    public PasswordSettingsModel PasswordSettingsModel { get; set; }
    public LockoutSettingsModel LockoutSettingsModel { get; set; }
    public SignInSettingsModel SignInSettingsModel { get; set; }

    #endregion Public Properties
}