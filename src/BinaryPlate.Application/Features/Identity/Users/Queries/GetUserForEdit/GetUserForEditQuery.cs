namespace BinaryPlate.Application.Features.Identity.Users.Queries.GetUserForEdit;

public class GetUserForEditQuery : IRequest<Envelope<GetUserForEditResponse>>
{
    #region Public Properties

    public string Id { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetUserForEditQueryHandler(ApplicationUserManager userManager) : IRequestHandler<GetUserForEditQuery, Envelope<GetUserForEditResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetUserForEditResponse>> Handle(GetUserForEditQuery request, CancellationToken cancellationToken)
        {
            // Check if request Id is null or white space and return BadRequest result if true.
            if (string.IsNullOrWhiteSpace(request.Id))
                return Envelope<GetUserForEditResponse>.Result.BadRequest(Resource.Invalid_user_Id);

            // Get the user entity from the database, including related entities for user roles and
            // user attachments.
            var user = await userManager.Users.Include(u => u.UserRoles)
                                               .ThenInclude(ur => ur.Role)
                                               .Include(u => u.UserAttachments)
                                               .Where(u => u.Id == request.Id)
                                               .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // Return NotFound result if user entity is null.
            if (user == null)
                return Envelope<GetUserForEditResponse>.Result.NotFound(Resource.Unable_to_load_user);

            // Map the assigned user roles and attachments to view models.
            var assignedRoles = user.UserRoles.Select(AssignedUserRoleItem.MapFromEntity).ToList();
            var assignedAttachments = user.UserAttachments.Select(AssignedUserAttachmentItem.MapFromEntity).ToList();

            // Map the user entity to the GetUserForEditResponse view model and return an Ok result.
            var userForEditResponse = GetUserForEditResponse.MapFromEntity(user, assignedRoles, assignedAttachments);
            return Envelope<GetUserForEditResponse>.Result.Ok(userForEditResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}