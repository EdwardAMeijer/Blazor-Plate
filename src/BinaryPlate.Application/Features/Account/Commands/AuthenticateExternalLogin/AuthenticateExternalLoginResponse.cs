namespace BinaryPlate.Application.Features.Account.Commands.AuthenticateExternalLogin;

public class AuthenticateExternalLoginResponse
{
    #region Public Properties

    public List<Claim> Claims { get; internal set; }

    #endregion Public Properties
}