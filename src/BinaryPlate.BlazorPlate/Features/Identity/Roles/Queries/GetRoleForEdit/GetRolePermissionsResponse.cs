namespace BinaryPlate.BlazorPlate.Features.Identity.Roles.Queries.GetRoleForEdit;

public class GetRolePermissionsResponse
{
    #region Public Properties

    public string RoleId { get; set; }
    public string Name { get; set; }
    public bool IsDefault { get; set; }
    public IList<PermissionItem> RequestedPermissions { get; set; }
    public IList<PermissionItem> SelectedPermissions { get; set; }

    #endregion Public Properties
}