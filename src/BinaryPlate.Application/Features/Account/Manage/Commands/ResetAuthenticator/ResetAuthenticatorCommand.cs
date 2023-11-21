namespace BinaryPlate.Application.Features.Account.Manage.Commands.ResetAuthenticator;

public class ResetAuthenticatorCommand : IRequest<Envelope<ResetAuthenticatorResponse>>
{
    #region Public Classes

    public class ConfirmEmailCommandHandler(ApplicationUserManager userManager,
                                            ITokenGeneratorService tokenGeneratorService,
                                            IHttpContextAccessor httpContextAccessor) : IRequestHandler<ResetAuthenticatorCommand, Envelope<ResetAuthenticatorResponse>>
    {
        #region Public Methods

        public async Task<Envelope<ResetAuthenticatorResponse>> Handle(ResetAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = httpContextAccessor.GetUserId();

            // Check if the user ID is null or empty.
            if (string.IsNullOrEmpty(userId))
                return Envelope<ResetAuthenticatorResponse>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user based on the user ID.
            var user = await userManager.FindByIdAsync(userId);

            // If the user is not found, return an unauthorized error.
            if (user == null)
                return Envelope<ResetAuthenticatorResponse>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Disable 2FA for the user.
            var setTwoFactorEnabledResult = await userManager.SetTwoFactorEnabledAsync(user, false);

            // If disabling 2FA fails, return an error.
            if (!setTwoFactorEnabledResult.Succeeded)
                return Envelope<ResetAuthenticatorResponse>.Result.ServerError(string.Format(Resource.Unable_to_enable_2FA, user.Id));

            // Reset the authenticator key for the user.
            var resetAuthenticatorKeyResult = await userManager.ResetAuthenticatorKeyAsync(user);

            // If resetting the authenticator key fails, return an error.
            if (!resetAuthenticatorKeyResult.Succeeded)
                return Envelope<ResetAuthenticatorResponse>.Result.ServerError(string.Format(Resource.Unable_to_reset_authenticator_keys, user.Id));

            // Generate a new authentication response for the user.
            var authResponse = await GenerateAuthResponseAsync(user);

            // Create a new response for resetting the authenticator key.
            var response = new ResetAuthenticatorResponse
            {
                // Set the success message for the response.
                StatusMessage = Resource.Your_authenticator_app_key_has_been_reset,
                // Set the new authentication response for the user.
                AuthResponse = authResponse
            };

            return Envelope<ResetAuthenticatorResponse>.Result.Ok(response);
        }

        #endregion Public Methods

        #region Private Methods

        private async Task<AuthResponse> GenerateAuthResponseAsync(ApplicationUser user)
        {
            // Generate an access token for the user.
            var accessToken = await tokenGeneratorService.GenerateAccessTokenAsync(user);

            // Generate a refresh token for the user.
            var refreshToken = tokenGeneratorService.GenerateRefreshToken();

            // Create a new AuthResponse object with the access token and refresh token.
            var response = new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return response;
        }

        #endregion Private Methods
    }

    #endregion Public Classes
}