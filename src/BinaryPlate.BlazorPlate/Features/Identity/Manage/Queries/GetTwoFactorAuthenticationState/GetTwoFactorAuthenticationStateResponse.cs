namespace BinaryPlate.BlazorPlate.Features.Identity.Manage.Queries.GetTwoFactorAuthenticationState;

public class GetTwoFactorAuthenticationStateResponse
{
    #region Public Properties

    public bool HasAuthenticator { get; set; }
    public int RecoveryCodesLeft { get; set; }
    public bool Is2FaEnabled { get; set; }
    public bool IsMachineRemembered { get; set; }

    #endregion Public Properties
}