namespace BinaryPlate.Application.Common.Models.Identity;

public class UserInfo
{
    #region Public Properties

    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DisplayName { get; set; }
    public string LoginProvider { get; set; }
    public string ProviderKey { get; set; }

    #endregion Public Properties
}