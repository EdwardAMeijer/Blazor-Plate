namespace BinaryPlate.Application.Features.Identity.Roles.Queries.GetRoles;

public class GetRolesResponse
{
    #region Public Properties

    public PagedList<RoleItem> Roles { get; set; }

    #endregion Public Properties
}