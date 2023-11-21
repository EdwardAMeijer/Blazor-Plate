namespace BinaryPlate.Application.Features.Account.Commands.LoginWith2fa;

public class LoginWith2FaCommand : IRequest<Envelope<LoginWith2FaResponse>>
{
    #region Public Properties

    public string UserName { get; set; }
    public string Password { get; set; }
    public string TwoFactorCode { get; set; }
    public bool RememberMachine { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class LoginWith2FaCommandHandler(ApplicationUserManager userManager,
                                            SignInManager<ApplicationUser> signInManager,
                                            IAuthService authService) : IRequestHandler<LoginWith2FaCommand, Envelope<LoginWith2FaResponse>>
    {
        #region Public Methods

        public async Task<Envelope<LoginWith2FaResponse>> Handle(LoginWith2FaCommand request, CancellationToken cancellationToken)
        {
            // Attempt to sign in the user with their email and password.
            var signInResult = await signInManager.PasswordSignInAsync(request.UserName, request.Password, false, lockoutOnFailure: true);

            // If the sign-in is successful and requires two-factor authentication,
            if (!signInResult.Succeeded && signInResult.RequiresTwoFactor)
            {
                // Find the user by email.
                var user = await userManager.FindByEmailAsync(request.UserName);

                // If user cannot be found, return a not found result with an error message.
                if (user == null)
                    return Envelope<LoginWith2FaResponse>.Result.NotFound(Resource.Invalid_login_attempt);

                // Remove any spaces or dashes from the two-factor code.
                var authenticatorCode = request.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

                // Verify the two-factor token.
                var isValidTwoFactorToken =
                    await userManager.VerifyTwoFactorTokenAsync(user,
                                                                 new IdentityOptions().Tokens.AuthenticatorTokenProvider,
                                                                 authenticatorCode);

                // If the token is valid, generate access and refresh tokens and return a successful
                // result with the authentication response.
                if (isValidTwoFactorToken)
                {
                    // Generate access and refresh tokens.
                    var (accessToken, refreshToken) = await authService.GenerateAccessAndRefreshTokens(user);

                    // Create an authorization response containing the tokens.
                    var authResponse = new AuthResponse
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                    };

                    // Create a response containing the authorization response.
                    var response = new LoginWith2FaResponse { AuthResponse = authResponse };

                    return Envelope<LoginWith2FaResponse>.Result.Ok(response);
                }
                // If the token is invalid, return a bad request result with an error message.
                return Envelope<LoginWith2FaResponse>.Result.BadRequest(Resource.Invalid_authenticator_code_entered);
            }

            // Otherwise, return an unsuccessful result with any sign-in errors.
            return Envelope<LoginWith2FaResponse>.Result.AddErrors(signInResult.ToApplicationResult(), HttpStatusCode.InternalServerError, rollbackDisabled: true);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}