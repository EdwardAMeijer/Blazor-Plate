namespace BinaryPlate.BlazorPlate.Features.Identity.Permissions.Queries.GetPermissions;

public class GetPermissionsResponse
{
    #region Public Properties

    public List<PermissionItem> Permissions { get; set; } = new();

    #endregion Public Properties
}