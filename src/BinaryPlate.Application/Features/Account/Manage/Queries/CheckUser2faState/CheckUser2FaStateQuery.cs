namespace BinaryPlate.Application.Features.Account.Manage.Queries.CheckUser2faState;

public class CheckUser2FaStateQuery : IRequest<Envelope<User2FaStateResponse>>
{
    #region Public Classes

    public class CheckUser2FaStateQueryHandler(ApplicationUserManager userManager,
                                               IHttpContextAccessor httpContextAccessor) : IRequestHandler<CheckUser2FaStateQuery, Envelope<User2FaStateResponse>>
    {
        #region Public Methods

        public async Task<Envelope<User2FaStateResponse>> Handle(CheckUser2FaStateQuery request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = httpContextAccessor.GetUserId();

            // Validate user id.
            if (string.IsNullOrEmpty(userId))
                return Envelope<User2FaStateResponse>.Result.BadRequest(Resource.Invalid_user_Id);

            // Retrieve user from UserManager.
            var user = await userManager.FindByIdAsync(userId);

            // Check if user exists.
            if (user == null)
                return Envelope<User2FaStateResponse>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Check if two-factor authentication is enabled for the user.
            var isTwoFactorEnabled = await userManager.GetTwoFactorEnabledAsync(user);

            // Create the response object.
            var response = new User2FaStateResponse
            {
                IsTwoFactorEnabled = isTwoFactorEnabled,
                StatusMessage = !isTwoFactorEnabled ? string.Format(Resource.Cannot_generate_recovery_codes, user.UserName) : string.Empty,
            };

            // Return the response object.
            return Envelope<User2FaStateResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}