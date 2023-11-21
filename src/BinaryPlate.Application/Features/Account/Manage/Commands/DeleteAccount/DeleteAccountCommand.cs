namespace BinaryPlate.Application.Features.Account.Manage.Commands.DeleteAccount;

public class DeleteAccountCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public string Password { get; set; }

    #endregion Public Properties

    public class DeleteAccountHandler(ApplicationUserManager userManager,
                                      SignInManager<ApplicationUser> signInManager,
                                      IHttpContextAccessor httpContextAccessor) : IRequestHandler<DeleteAccountCommand, Envelope<string>>
    {
        #region Public Methods

        #region Public Methods

        public async Task<Envelope<string>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = httpContextAccessor.GetUserId();

            // If the user ID is null or empty, return a bad request result with an error message.
            if (string.IsNullOrEmpty(userId))
                return Envelope<string>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user by ID.
            var user = await userManager.FindByIdAsync(userId);

            // If the user is null, return an unauthorized result with an error message.
            if (user == null)
                return Envelope<string>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Check if the user requires a password to delete their personal data.
            var requirePassword = await userManager.HasPasswordAsync(user);

            // If a password is required, check if the provided password is correct.
            if (requirePassword)
                if (!await userManager.CheckPasswordAsync(user, request.Password))
                    return Envelope<string>.Result.ServerError(Resource.Incorrect_password);

            // Delete the user.
            var identityResult = await userManager.DeleteAsync(user);

            // If the delete operation failed, return a server error result with an error message.
            if (!identityResult.Succeeded)
                return Envelope<string>.Result.ServerError(string.Format(Resource.Unexpected_error_occurred_deleting_user_with_Id, user.Id));

            // Sign the user out.
            await signInManager.SignOutAsync();

            // Return an OK result with a success message.
            return Envelope<string>.Result.Ok(string.Format(Resource.User_with_Id_deleted, userId));
        }

        #endregion Public Methods
    }

    #endregion Public Methods
}