namespace BinaryPlate.Application.Features.Identity.Roles.Queries.GetRoleForEdit;

public class GetRoleForEditResponse : AuditableDto
{
    #region Public Properties

    public string Id { get; set; }
    public bool IsDefault { get; set; }
    public string Name { get; set; }
    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static GetRoleForEditResponse MapFromEntity(ApplicationRole role)
    {
        return new GetRoleForEditResponse
        {
            Id = role.Id,
            Name = role.Name,
            IsDefault = role.IsDefault,
            ConcurrencyStamp = role.ConcurrencyStamp,
            CreatedOn = role.CreatedOn,
            CreatedBy = role.CreatedBy,
            ModifiedOn = role.ModifiedOn,
            ModifiedBy = role.ModifiedBy
        };
    }

    #endregion Public Methods
}