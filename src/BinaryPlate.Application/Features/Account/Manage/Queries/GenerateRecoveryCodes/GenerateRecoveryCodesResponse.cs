namespace BinaryPlate.Application.Features.Account.Manage.Queries.GenerateRecoveryCodes;

public class GenerateRecoveryCodesResponse
{
    #region Public Properties

    public IEnumerable<string> RecoveryCodes { get; set; } = new List<string>();
    public string StatusMessage { get; set; }

    #endregion Public Properties
}