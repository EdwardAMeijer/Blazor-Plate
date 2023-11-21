namespace BinaryPlate.Application.Features.AppSettings.Commands.UpdateSettings.UpdateIdentitySettings.Models;

public class SignInSettingsModel
{
    #region Public Properties

    public string Id { get; set; }

    //public bool RequireConfirmedEmail { get; set; }
    //public bool RequireConfirmedPhoneNumber { get; set; }
    public bool RequireConfirmedAccount { get; set; }

    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties
}