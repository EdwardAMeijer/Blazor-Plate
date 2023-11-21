namespace BinaryPlate.BlazorPlate.Contracts.Providers;

public interface IAccessTokenProvider
{
    #region Public Methods

    Task<string> TryGetAccessToken();

    #endregion Public Methods
}