namespace BinaryPlate.BlazorPlate.Contracts.Providers;

public interface IReturnUrlProvider
{
    #region Public Methods

    Task<string> GetReturnUrl();

    Task SetReturnUrl(string returnUrl);

    Task RemoveReturnUrl();

    #endregion Public Methods
}