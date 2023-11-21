namespace BinaryPlate.Application.Features.Account.Manage.Commands.EnableAuthenticator;

public class EnableAuthenticatorCommand : IRequest<Envelope<EnableAuthenticatorResponse>>
{
    #region Public Properties

    public string Code { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class EnableAuthenticatorCommandHandler(ApplicationUserManager userManager,
                                                   IHttpContextAccessor httpContextAccessor) : IRequestHandler<EnableAuthenticatorCommand, Envelope<EnableAuthenticatorResponse>>
    {
        #region Public Methods

        public async Task<Envelope<EnableAuthenticatorResponse>> Handle(EnableAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = httpContextAccessor.GetUserId();

            // Validate user id.
            if (string.IsNullOrEmpty(userId))
                return Envelope<EnableAuthenticatorResponse>.Result.BadRequest(Resource.Invalid_user_Id);

            // Retrieve user from UserManager.
            var user = await userManager.FindByIdAsync(userId);

            // Check if user exists.
            if (user == null)
                return Envelope<EnableAuthenticatorResponse>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Remove spaces and hypens from the provided code.
            var verificationCode = request.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            // Verify the 2FA token.
            var is2FaTokenValid = await userManager.VerifyTwoFactorTokenAsync(
                user, userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            // Check if the token is valid.
            if (!is2FaTokenValid)
                return Envelope<EnableAuthenticatorResponse>.Result.ServerError(Resource.Verification_code_is_invalid);

            // Enable two-factor authentication for the user.
            var identityResult = await userManager.SetTwoFactorEnabledAsync(user, true);

            // Check if the operation succeeded.
            if (!identityResult.Succeeded)
                return Envelope<EnableAuthenticatorResponse>.Result.ServerError(string.Format(Resource.Unable_to_enable_2FA, user.Id));

            // Create the response object.
            var response = new EnableAuthenticatorResponse();

            // Check if the user has any recovery codes.
            if (await userManager.CountRecoveryCodesAsync(user) == 0)
            {
                // Generate new recovery codes.
                var recoveryCodes = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
                response.RecoveryCodes = (recoveryCodes ?? throw new InvalidOperationException()).ToArray();
                response.ShowRecoveryCodes = true; //If true, RedirectToPage("./ShowRecoveryCodes")
            }
            else
            {
                response.ShowRecoveryCodes = false; //If false, RedirectToPage("./TwoFactorAuthentication");
            }

            response.SuccessMessage = Resource.Your_authenticator_app_has_been_verified;

            // Return the response object.
            return Envelope<EnableAuthenticatorResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}