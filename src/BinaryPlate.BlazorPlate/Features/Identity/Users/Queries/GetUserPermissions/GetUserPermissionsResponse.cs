namespace BinaryPlate.BlazorPlate.Features.Identity.Users.Queries.GetUserPermissions;

public class GetUserPermissionsResponse
{
    #region Public Properties

    public IList<PermissionItem> SelectedPermissions { get; set; } = new List<PermissionItem>();
    public IList<PermissionItem> RequestedPermissions { get; set; } = new List<PermissionItem>();

    #endregion Public Properties
}