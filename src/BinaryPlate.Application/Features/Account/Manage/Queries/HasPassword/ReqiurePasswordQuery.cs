namespace BinaryPlate.Application.Features.Account.Manage.Queries.HasPassword;

public class RequirePasswordQuery : IRequest<Envelope<bool>>
{
    #region Public Classes

    public class RequirePasswordQueryHandler(ApplicationUserManager userManager,
                                             IHttpContextAccessor httpContextAccessor) : IRequestHandler<RequirePasswordQuery, Envelope<bool>>
    {
        #region Public Methods

        public async Task<Envelope<bool>> Handle(RequirePasswordQuery request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = httpContextAccessor.GetUserId();

            // If the user ID is null or empty, return a bad request result with a message
            // indicating an invalid user ID.
            if (string.IsNullOrEmpty(userId))
                return Envelope<bool>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user by their ID.
            var user = await userManager.FindByIdAsync(userId);

            // If the user cannot be found, return an unauthorized result with a message indicating
            // that the user cannot be loaded.
            return user == null
                ? Envelope<bool>.Result.Unauthorized(Resource.Unable_to_load_user)
                : Envelope<bool>.Result.Ok(await userManager.HasPasswordAsync(user));
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}