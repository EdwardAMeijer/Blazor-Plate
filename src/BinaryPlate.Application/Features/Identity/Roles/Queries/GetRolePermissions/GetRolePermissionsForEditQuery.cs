namespace BinaryPlate.Application.Features.Identity.Roles.Queries.GetRolePermissions;

public class GetRolePermissionsForEditQuery : IRequest<Envelope<GetRolePermissionsResponse>>
{
    #region Public Properties

    public string RoleId { get; set; }
    public Guid? PermissionId { get; set; }
    public bool LoadingOnDemand { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class GetRolePermissionsForEditQueryHandler(IApplicationDbContext dbContext, 
                                                       IPermissionService permissionService) : IRequestHandler<GetRolePermissionsForEditQuery, Envelope<GetRolePermissionsResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetRolePermissionsResponse>> Handle(GetRolePermissionsForEditQuery request, CancellationToken cancellationToken)
        {
            // Checks if the RoleId provided in the request is valid.
            if (string.IsNullOrWhiteSpace(request.RoleId))
                return Envelope<GetRolePermissionsResponse>.Result.BadRequest(Resource.Invalid_role_Id);

            // Retrieves the role from the database.
            var role = await dbContext.Roles.Include(r => r.RoleClaims)
                                             .Where(r => r.Id == request.RoleId)
                                             .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // Checks if the role was found.
            if (role == null)
                return Envelope<GetRolePermissionsResponse>.Result.NotFound(Resource.Unable_to_load_role);

            // Retrieves all the application permissions from the database.
            var permissions = await dbContext.ApplicationPermissions.ToListAsync(cancellationToken: cancellationToken);

            // Selects the permissions that belong to the role and maps them to PermissionItem objects.
            var selectedPermissions = role.RoleClaims.Join(permissions, rc => rc.ClaimValue, p => p.Name, (rc, p) => new PermissionItem
            {
                Id = p.Id,
                Name = rc.ClaimValue,
            }).ToList();

            List<PermissionItem> requestedPermissions;

            // Checks if the permissions are being loaded on demand or all at once.
            if (request.LoadingOnDemand)
            {
                // Retrieves the requested permissions and maps them to PermissionItem objects.
                var loadedOnDemandPermissions = await permissionService.GetPermissionsOnDemand(new GetPermissionsQuery { Id = request.PermissionId });
                requestedPermissions = loadedOnDemandPermissions.Payload.Permissions;
            }
            else
            {
                // Retrieves all the application permissions and maps them to PermissionItem objects.
                var loadedOneShotPermissions = await permissionService.GetAllPermissions();
                requestedPermissions = loadedOneShotPermissions.Payload.Permissions;
            }

            // Maps the role and the selected/requested permissions to a GetRolePermissionsResponse object.
            var rolePermissionsForEditResponse = GetRolePermissionsResponse.MapFromEntity(role, selectedPermissions, requestedPermissions);

            // Returns the mapped role and permissions in an Envelope object with the Ok status.
            return Envelope<GetRolePermissionsResponse>.Result.Ok(rolePermissionsForEditResponse);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}