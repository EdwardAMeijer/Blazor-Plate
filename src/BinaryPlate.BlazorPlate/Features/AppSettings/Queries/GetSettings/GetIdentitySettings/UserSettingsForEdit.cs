namespace BinaryPlate.BlazorPlate.Features.AppSettings.Queries.GetSettings.GetIdentitySettings;

public class UserSettingsForEdit
{
    #region Public Properties

    public string Id { get; set; }
    public string AllowedUserNameCharacters { get; set; }
    public bool NewUsersActiveByDefault { get; set; }
    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties

    #region Public Methods

    public UserSettingsModel MapToCommand()
    {
        return new UserSettingsModel
        {
            Id = Id,
            AllowedUserNameCharacters = AllowedUserNameCharacters,
            NewUsersActiveByDefault = NewUsersActiveByDefault,
            ConcurrencyStamp = ConcurrencyStamp
        };
    }

    #endregion Public Methods
}