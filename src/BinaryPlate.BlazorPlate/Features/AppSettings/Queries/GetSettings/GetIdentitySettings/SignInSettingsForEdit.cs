namespace BinaryPlate.BlazorPlate.Features.AppSettings.Queries.GetSettings.GetIdentitySettings;

public class SignInSettingsForEdit
{
    #region Public Properties

    public string Id { get; set; }

    //public bool RequireConfirmedEmail { get; set; }
    //public bool RequireConfirmedPhoneNumber { get; set; }
    public bool RequireConfirmedAccount { get; set; }

    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties

    #region Public Methods

    public SignInSettingsModel MapToCommand()
    {
        return new SignInSettingsModel
        {
            Id = Id,
            RequireConfirmedAccount = RequireConfirmedAccount,
            ConcurrencyStamp = ConcurrencyStamp
        };
    }

    #endregion Public Methods
}