namespace BinaryPlate.Application.Features.Identity.Roles.Queries.GetRolePermissions;

public class GetRolePermissionsResponse : AuditableDto
{
    #region Public Properties

    public string RoleId { get; set; }
    public IReadOnlyList<PermissionItem> RequestedPermissions { get; set; } = new List<PermissionItem>();
    public IReadOnlyList<PermissionItem> SelectedPermissions { get; set; } = new List<PermissionItem>();

    #endregion Public Properties

    #region Public Methods

    public static GetRolePermissionsResponse MapFromEntity(ApplicationRole role, List<PermissionItem> selectedPermissions, List<PermissionItem> requestedPermissions)
    {
        return new()
        {
            RoleId = role.Id,
            SelectedPermissions = selectedPermissions,
            RequestedPermissions = requestedPermissions,
        };
    }

    #endregion Public Methods
}