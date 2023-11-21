namespace BinaryPlate.Application.Features.Identity.Users.Queries.GetUserPermissions;

public class GetUserPermissionsQuery : IRequest<Envelope<GetUserPermissionsResponse>>
{
    #region Public Properties

    public string UserId { get; set; }
    public bool LoadingOnDemand { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetUserPermissionsQueryHandler(ApplicationUserManager userManager, IPermissionService permissionService) : IRequestHandler<GetUserPermissionsQuery, Envelope<GetUserPermissionsResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetUserPermissionsResponse>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
        {
            // Find the user by their ID using the user manager and include their claims.
            var user = await userManager.Users.Include(u => u.Claims)
                                               .Where(u => u.Id == request.UserId)
                                               .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // If the user is not found, return a NotFound response with an error message.
            if (user == null)
                return Envelope<GetUserPermissionsResponse>.Result.NotFound(Resource.Unable_to_load_user);

            // Get the selected non-excluded permissions for the user.
            var userPermissionsExceptExcluded = await permissionService.GetUserPermissionsExceptExcluded(user);

            // Declare a list of all permissions.
            IReadOnlyList<PermissionItem> allPermissions;

            // If loading on demand is requested, get the permissions on demand and set them to allPermissions.
            if (request.LoadingOnDemand)
            {
                var loadedOnDemandPermissions = await permissionService.GetPermissionsOnDemand(new GetPermissionsQuery());
                allPermissions = loadedOnDemandPermissions.Payload.Permissions;
            }
            // If not, get all permissions at once and set them to allPermissions.
            else
            {
                var loadedOneShotPermissions = await permissionService.GetAllPermissions();
                allPermissions = loadedOneShotPermissions.Payload.Permissions;
            }

            // Create a new GetUserPermissionsResponse with the selected and requested permissions,
            // and return an OK response with it.
            var response = new GetUserPermissionsResponse
            {
                SelectedPermissions = userPermissionsExceptExcluded,
                RequestedPermissions = allPermissions
            };
            // Return the user non-excluded permissions.
            return Envelope<GetUserPermissionsResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}