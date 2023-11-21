﻿namespace BinaryPlate.Application.Features.Account.Manage.Commands.ConfirmEmailChange;

public class ConfirmEmailChangeCommand : IRequest<Envelope<ChangeEmailResponse>>
{
    #region Public Properties

    public string UserId { get; set; }
    public string Email { get; set; }
    public string Code { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class ConfirmEmailChangeCommandHandler(ApplicationUserManager userManager,
                                                  ITokenGeneratorService tokenGeneratorService) : IRequestHandler<ConfirmEmailChangeCommand, Envelope<ChangeEmailResponse>>
    {
        #region Public Methods

        public async Task<Envelope<ChangeEmailResponse>> Handle
            (ConfirmEmailChangeCommand request, CancellationToken cancellationToken)
        {
            // Check if user ID is provided in the request.
            if (string.IsNullOrEmpty(request.UserId))
                return Envelope<ChangeEmailResponse>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find user by ID.
            var user = await userManager.FindByIdAsync(request.UserId);

            ChangeEmailResponse response;

            // If user is not found, return success response without revealing that the user does
            // not exist.
            if (user == null)
            {
                response = new ChangeEmailResponse
                {
                    RequireConfirmedAccount = false,
                    DisplayConfirmAccountLink = false,
                    EmailConfirmationUrl = null,
                    SuccessMessage = Resource.Your_email_has_been_changed_successfully,
                };

                return Envelope<ChangeEmailResponse>.Result.Ok(response);
            }

            // Decode the email change code.
            request.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));

            // Change the user's email address.
            var changeEmailResult = await userManager.ChangeEmailAsync(user, request.Email, request.Code);

            // If email change fails, return the error response.
            if (!changeEmailResult.Succeeded)
                return Envelope<ChangeEmailResponse>.Result.AddErrors(changeEmailResult.Errors.ToApplicationResult(),
                                                                      HttpStatusCode.InternalServerError);

            // In our UI, email and user name are one and the same, so when we update the email we
            // need to update the user name. Change the user's username to match the new email address.
            var setUserNameResult = await userManager.SetUserNameAsync(user, request.Email);

            // If username change fails, return the error response.
            if (!setUserNameResult.Succeeded)
                return Envelope<ChangeEmailResponse>.Result.ServerError(Resource.Error_changing_your_user_name);

            // Generate a new access and refresh token for the user.
            var authResponse = await GenerateAuthResponseAsync(user);

            // Create a success response with the new auth token and message.
            response = new ChangeEmailResponse
            {
                RequireConfirmedAccount = false,
                DisplayConfirmAccountLink = false,
                EmailConfirmationUrl = null,
                SuccessMessage = Resource.Your_email_has_been_changed_successfully,
                AuthResponse = authResponse
            };

            // Return the success response.
            return Envelope<ChangeEmailResponse>.Result.Ok(response);
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