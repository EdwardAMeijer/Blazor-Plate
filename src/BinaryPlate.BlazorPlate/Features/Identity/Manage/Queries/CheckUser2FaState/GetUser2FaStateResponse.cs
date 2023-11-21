namespace BinaryPlate.BlazorPlate.Features.Identity.Manage.Queries.CheckUser2FaState;

public class GetUser2FaStateResponse
{
    #region Public Properties

    public bool IsTwoFactorEnabled { get; set; }
    public string StatusMessage { get; set; }

    #endregion Public Properties
}