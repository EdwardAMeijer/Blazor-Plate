namespace BinaryPlate.Application.Features.AppSettings.Commands.UpdateSettings.UpdateIdentitySettings;

public class GetIdentitySettingsResponse
{
    #region Public Properties

    public Guid UserSettingsId { get; set; }
    public Guid PasswordSettingsId { get; set; }
    public Guid LockoutSettingsId { get; set; }
    public Guid SignInSettingsId { get; set; }

    public string LockoutSettingsConcurrencyStamp { get; set; }
    public string PasswordSettingsConcurrencyStamp { get; set; }
    public string SignInSettingsConcurrencyStamp { get; set; }
    public string UserSettingsConcurrencyStamp { get; set; }

    public string SuccessMessage { get; set; }

    #endregion Public Properties
}