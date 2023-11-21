namespace BinaryPlate.BlazorPlate.Features.Identity.Users.Queries.GetUsers;

public class GetUsersResponse
{
    #region Public Properties

    public PagedList<UserItem> Users { get; set; }

    #endregion Public Properties
}