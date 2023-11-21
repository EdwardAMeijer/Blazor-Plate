namespace BinaryPlate.Application.Features.Account.Commands.RegisterExternalLogin;

public class RegisterExternalLoginResponse
{
    #region Public Properties

    public bool RequiresTwoFactor { get; set; }
    public AuthResponse AuthResponse { get; set; }

    #endregion Public Properties
}