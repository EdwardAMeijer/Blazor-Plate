namespace BinaryPlate.Application.Features.AppSettings.Commands.UpdateSettings.UpdateIdentitySettings.Models;

public class UserSettingsModel
{
    #region Public Properties

    public string Id { get; set; }
    public string AllowedUserNameCharacters { get; set; }
    public bool NewUsersActiveByDefault { get; set; }
    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties
}