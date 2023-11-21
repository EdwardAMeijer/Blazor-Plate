namespace BinaryPlate.Application.Features.Identity.Roles.Commands.CreateRole;

public class CreateRoleCommand : IRequest<Envelope<CreateRoleResponse>>
{
    #region Public Properties

    public string Name { get; set; }
    public bool IsDefault { get; set; }
    public IReadOnlyList<Guid> SelectedPermissionIds { get; set; }

    #endregion Public Properties

    #region Public Methods

    public ApplicationRole MapToEntity()
    {
        return new()
        {
            IsDefault = IsDefault,
            Name = Name
        };
    }

    #endregion Public Methods

    #region Public Classes

    public class CreateRoleCommandHandler(ApplicationRoleManager roleManager, 
                                          IApplicationDbContext dbContext) : IRequestHandler<CreateRoleCommand, Envelope<CreateRoleResponse>>
    {
        #region Public Methods

        public async Task<Envelope<CreateRoleResponse>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            // Map the request to an entity (i.e. create a new role)
            var role = request.MapToEntity();

            // Get the selected permissions for the role.
            var selectedPermissions = dbContext.ApplicationPermissions.Where(p => request.SelectedPermissionIds.Contains(p.Id));

            // Add each selected permission as a new role claim for the role.
            foreach (var selectedPermission in selectedPermissions)
                role.RoleClaims.Add(new ApplicationRoleClaim { ClaimType = "permissions", ClaimValue = selectedPermission.Name });

            // Create the new role.
            var identityResult = await roleManager.CreateAsync(role);

            // If the creation of the role was unsuccessful, return an Envelope with the errors.
            if (!identityResult.Succeeded)
                return Envelope<CreateRoleResponse>.Result.AddErrors(identityResult.Errors.ToApplicationResult(), HttpStatusCode.InternalServerError);

            // Otherwise, create the response with the new role's ID and a success message.
            var response = new CreateRoleResponse
            {
                Id = role.Id,
                SuccessMessage = Resource.Role_has_been_created_successfully
            };

            // Return the response in an Envelope with a success status.
            return Envelope<CreateRoleResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}