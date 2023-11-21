namespace BinaryPlate.Application.Features.Account.Manage.Commands.Disable2fa;

public class Disable2FaCommand : IRequest<Envelope<string>>
{
    #region Public Classes

    public class Disable2FaCommandHandler(ApplicationUserManager userManager, IHttpContextAccessor httpContextAccessor) : IRequestHandler<Disable2FaCommand, Envelope<string>>
    {
        #region Public Methods

        public async Task<Envelope<string>> Handle(Disable2FaCommand request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = httpContextAccessor.GetUserId();

            // If the user ID is missing or invalid, return a bad request response.
            if (string.IsNullOrEmpty(userId))
                return Envelope<string>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user with the specified ID.
            var user = await userManager.FindByIdAsync(userId);

            // If the user cannot be found, return an unauthorized response.
            if (user == null)
                return Envelope<string>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Disable two-factor authentication for the user.
            var identityResult = await userManager.SetTwoFactorEnabledAsync(user, false);

            // If an error occurred while disabling 2FA, return a server error response.
            if (!identityResult.Succeeded)
                return Envelope<string>.Result.ServerError(string.Format(Resource.Unexpected_error_occurred_disabling_2FA, user.Id));

            // Return a success response.
            return Envelope<string>.Result.Ok(Resource.Two_factor_authentication_has_been_disabled);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}