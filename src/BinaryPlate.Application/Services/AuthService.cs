namespace BinaryPlate.Application.Services;

public class AuthService(ApplicationUserManager userManager,
                         ITokenGeneratorService tokenGeneratorService,
                         SignInManager<ApplicationUser> signInManager,
                         IAppSettingsService appSettingsService) : IAuthService
{
    #region Public Methods

    public async Task<Envelope<LoginResponse>> Login(LoginCommand request)
    {
        // Attempt to sign in the user with their email and password.
        var signInResult = await signInManager.PasswordSignInAsync(request.Email,
                                                                    request.Password,
                                                                    isPersistent: false,
                                                                    lockoutOnFailure: true);

        // If the sign-in is successful.
        if (signInResult.Succeeded)
        {
            // Find the user by email.
            var user = await userManager.FindByNameAsync(request.Email);

            // If user cannot be found, return a server error with an error message.
            if (user == null)
                return Envelope<LoginResponse>.Result.ServerError(Resource.Invalid_login_attempt);

            // If user is suspended, return a server error with a deactivation message.
            if (user.IsSuspended)
                return
                    Envelope<LoginResponse>.Result
                                           .ServerError(Resource.Your_account_is_deactivated_Please_contact_your_administrator,
                                                        rollbackDisabled: true);

            // Generate access and refresh tokens for the user.
            var (accessToken, refreshToken) = await GenerateAccessAndRefreshTokens(user);

            // Create an authentication response with the access and refresh tokens.
            var authResponse = new AuthResponse { AccessToken = accessToken, RefreshToken = refreshToken };

            // Create a login response with the authentication response and a flag indicating
            // whether two-factor authentication is required.
            var response = new LoginResponse
            {
                AuthResponse = authResponse,
                RequiresTwoFactor = false,
            };

            // Return a successful result with the login response.
            return Envelope<LoginResponse>.Result.Ok(response);
        }

        // If the sign-in requires two-factor authentication, return a successful result indicating this.
        if (signInResult.RequiresTwoFactor)
            return Envelope<LoginResponse>.Result.Ok(new LoginResponse { RequiresTwoFactor = true });

        // Otherwise, return an unsuccessful result with any sign-in errors.
        return Envelope<LoginResponse>.Result.AddErrors(signInResult.ToApplicationResult(),
                                                        HttpStatusCode.InternalServerError, rollbackDisabled: true);
    }

    public async Task<(string accessToken, string refreshToken)> GenerateAccessAndRefreshTokens(ApplicationUser user, bool hasPassword = true)
    {
        // Generate a new refresh token for the user.
        var refreshToken = tokenGeneratorService.GenerateRefreshToken();

        // Store the refresh token in the user's RefreshToken property.
        user.RefreshToken = refreshToken;

        // Get token settings from app settings.
        var tokenSettingsResponse = await appSettingsService.GetTokenSettings();

        // Get the RefreshTokenTimeSpan from token settings.
        var refreshTokenTimeSpan = tokenSettingsResponse.Payload.RefreshTokenTimeSpan ??
                                   throw new ArgumentNullException(Resource.Refresh_token_timespan_cannot_be_null);

        // Set the RefreshTokenTimeSpan in the user's RefreshTokenTimeSpan property.
        user.RefreshTokenTimeSpan = DateTime.UtcNow.AddMinutes(refreshTokenTimeSpan);

        // Update the user in the database.
        await userManager.UpdateAsync(user);

        // Generate an access token for the user.
        var accessToken = await tokenGeneratorService.GenerateAccessTokenAsync(user, hasPassword);

        // Return the access token and refresh token as a tuple.
        return (accessToken, refreshToken);
    }

    #endregion Public Methods
}