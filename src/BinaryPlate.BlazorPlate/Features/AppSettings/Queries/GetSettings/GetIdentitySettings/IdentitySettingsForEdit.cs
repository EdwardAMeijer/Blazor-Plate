namespace BinaryPlate.BlazorPlate.Features.AppSettings.Queries.GetSettings.GetIdentitySettings;

public class IdentitySettingsForEdit
{
    #region Public Properties

    public UserSettingsForEdit UserSettingsForEdit { get; set; } = new();
    public PasswordSettingsForEdit PasswordSettingsForEdit { get; set; } = new();
    public LockoutSettingsForEdit LockoutSettingsForEdit { get; set; } = new();
    public SignInSettingsForEdit SignInSettingsForEdit { get; set; } = new();

    #endregion Public Properties
}