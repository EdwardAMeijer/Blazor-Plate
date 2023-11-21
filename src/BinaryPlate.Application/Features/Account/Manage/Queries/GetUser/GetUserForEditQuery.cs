namespace BinaryPlate.Application.Features.Account.Manage.Queries.GetUser;

public class GetCurrentUserForEditQuery : IRequest<Envelope<CurrentUserForEditResponse>>
{
    #region Public Classes

    public class GetCurrentUserForEditQueryHandler(ApplicationUserManager userManager, 
                                                   IHttpContextAccessor httpContextAccessor) : IRequestHandler<GetCurrentUserForEditQuery, Envelope<CurrentUserForEditResponse>>
    {
        #region Public Methods

        public async Task<Envelope<CurrentUserForEditResponse>> Handle(GetCurrentUserForEditQuery request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = httpContextAccessor.GetUserId();

            // Return BadRequest if user ID is null or empty.
            if (string.IsNullOrEmpty(userId))
                return Envelope<CurrentUserForEditResponse>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user by their ID.
            var user = await userManager.FindByIdAsync(userId);

            // Return Unauthorized if user is null.
            if (user == null)
                return Envelope<CurrentUserForEditResponse>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Create a CurrentUserForEditResponse object with the user's information.
            var response = new CurrentUserForEditResponse
            {
                Username = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                JobTitle = user.JobTitle,
                AvatarUri = user.AvatarUri
            };

            // Return Ok with the CurrentUserForEditResponse object.
            return Envelope<CurrentUserForEditResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}