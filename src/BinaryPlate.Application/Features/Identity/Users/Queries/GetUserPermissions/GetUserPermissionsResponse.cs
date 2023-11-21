namespace BinaryPlate.Application.Features.Identity.Users.Queries.GetUserPermissions;

public class GetUserPermissionsResponse
{
    #region Public Properties

    public IReadOnlyList<PermissionItem> SelectedPermissions { get; set; } = new List<PermissionItem>();
    public IReadOnlyList<PermissionItem> RequestedPermissions { get; set; } = new List<PermissionItem>();

    #endregion Public Properties
}