namespace BinaryPlate.Application.Features.Account.Manage.Commands.ChangeEmail;

public class ChangeEmailCommand : IRequest<Envelope<ChangeEmailResponse>>
{
    #region Public Properties

    public string NewEmail { get; set; }
    public bool DisplayConfirmAccountLink { get; set; } = true;

    #endregion Public Properties

    #region Public Classes

    public class ChangeEmailCommandHandler(ApplicationUserManager userManager,
                                           ITokenGeneratorService tokenGeneratorService,
                                           IHttpContextAccessor httpContextAccessor) : IRequestHandler<ChangeEmailCommand, Envelope<ChangeEmailResponse>>
    {
        #region Public Methods

        public async Task<Envelope<ChangeEmailResponse>> Handle
            (ChangeEmailCommand request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = httpContextAccessor.GetUserId();

            // Return a bad request error if the user ID is null or empty.
            if (string.IsNullOrEmpty(userId))
                return Envelope<ChangeEmailResponse>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user based on the retrieved ID.
            var user = await userManager.FindByIdAsync(httpContextAccessor.GetUserId());

            // If the user cannot be found, return an unauthorized error.
            if (user == null)
                return Envelope<ChangeEmailResponse>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Get the current email address of the user.
            var email = await userManager.GetEmailAsync(user);

            ChangeEmailResponse response;

            // If the new email address is the same as the current email address, return a success
            // message with the current email address.
            if (request.NewEmail == email)
            {
                response = new ChangeEmailResponse
                {
                    RequireConfirmedAccount = false,
                    EmailUnchanged = true,
                    EmailConfirmationUrl = null,
                    AuthResponse = null,
                    SuccessMessage = Resource.Your_email_is_unchanged
                };
                return Envelope<ChangeEmailResponse>.Result.Ok(response);
            }

            // If confirmed account is required, generate a confirmation link and a token response
            // for authentication.
            if (userManager.Options.SignIn.RequireConfirmedAccount)
            {
                var callbackUrl = await userManager.SendActivationEmailAsync(user, request.NewEmail);

                var tokenResponse = await GenerateAuthResponseAsync(user);

                // If the token response is null, return a bad request error with a message.
                if (tokenResponse == null)
                    return Envelope<ChangeEmailResponse>.Result.BadRequest(string.Format(Resource.value_cannot_be_null, nameof(tokenResponse)));

                //If the request specifies to display the confirm account link, return the link along with the token response; otherwise, return only the link.
                if (request.DisplayConfirmAccountLink)
                {
                    response = new ChangeEmailResponse
                    {
                        RequireConfirmedAccount = true,
                        DisplayConfirmAccountLink = true,
                        EmailConfirmationUrl = HttpUtility.UrlEncode(callbackUrl),
                        AuthResponse = tokenResponse,
                        SuccessMessage = Resource.Confirmation_link_to_change_email_has_been_sent
                    };
                    return Envelope<ChangeEmailResponse>.Result.Ok(response);
                }

                response = new ChangeEmailResponse
                {
                    RequireConfirmedAccount = true,
                    DisplayConfirmAccountLink = false,
                    EmailConfirmationUrl = HttpUtility.UrlEncode(callbackUrl),
                    SuccessMessage = Resource.Confirmation_link_to_change_email_has_been_sent,
                };

                return Envelope<ChangeEmailResponse>.Result.Ok(response);
            }

            // If confirmed account is not required, simply update the user name and email and
            // return the result.
            return await UpdateUserNameAndEmail(user, request.NewEmail);
        }

        #endregion Public Methods

        #region Private Methods

        private async Task<Envelope<ChangeEmailResponse>> UpdateUserNameAndEmail(ApplicationUser user, string email)
        {
            // Update the user's email.
            user.Email = email;

            // Update the user's information in the database.
            var updateUserResult = await userManager.UpdateAsync(user);

            // If the update was not successful, return an error.
            if (!updateUserResult.Succeeded)
                return Envelope<ChangeEmailResponse>.Result.AddErrors(updateUserResult.Errors.ToApplicationResult(),
                                                                      HttpStatusCode.InternalServerError);

            // In our UI, email and user name are one and the same, so when we update the email we
            // need to update the user name.
            var setUserNameResult = await userManager.SetUserNameAsync(user, email);

            // If the update was not successful, return an error.
            if (!setUserNameResult.Succeeded)
                return Envelope<ChangeEmailResponse>.Result.ServerError(Resource.Error_changing_user_name);

            // Generate an authentication response for the user.
            var authResponse = await GenerateAuthResponseAsync(user);

            // Create a new ChangeEmailResponse object with the new email and authentication response.
            var response = new ChangeEmailResponse
            {
                RequireConfirmedAccount = false,
                DisplayConfirmAccountLink = false,
                EmailConfirmationUrl = null,
                SuccessMessage = Resource.Your_email_has_been_successfully_changed,
                AuthResponse = authResponse
            };

            return Envelope<ChangeEmailResponse>.Result.Ok(response);
        }

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