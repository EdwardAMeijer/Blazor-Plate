namespace BinaryPlate.Application.Features.Account.Manage.Commands.SetPassword;

public class SetPasswordCommand : IRequest<Envelope<SetPasswordResponse>>
{
    #region Public Properties

    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class SetPasswordCommandHandler(ApplicationUserManager userManager,
                                           IHttpContextAccessor httpContextAccessor) : IRequestHandler<SetPasswordCommand, Envelope<SetPasswordResponse>>
    {
        #region Public Methods

        public async Task<Envelope<SetPasswordResponse>> Handle(SetPasswordCommand request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = httpContextAccessor.GetUserId();

            // Return a bad request result if the user ID is invalid.
            if (string.IsNullOrEmpty(userId))
                return Envelope<SetPasswordResponse>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user based on the ID.
            var user = await userManager.FindByIdAsync(userId);

            // Return an unauthorized result if the user is not found.
            if (user == null)
                return Envelope<SetPasswordResponse>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Add the new password to the user's account.
            var identityResult = await userManager.AddPasswordAsync(user, request.NewPassword);

            // Return a server error result if the password cannot be added.
            if (!identityResult.Succeeded)
                return Envelope<SetPasswordResponse>.Result.AddErrors(identityResult.Errors.ToApplicationResult(), HttpStatusCode.InternalServerError);

            // Create the response object with the success message.
            var response = new SetPasswordResponse
            {
                SuccessMessage = $"{Resource.Your_password_has_been_set} {Resource.Please_sign_in_to_your_account_with_your_new_password}"
            };

            // Return the response object with a successful result.
            return Envelope<SetPasswordResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}