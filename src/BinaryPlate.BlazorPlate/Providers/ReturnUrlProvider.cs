namespace BinaryPlate.BlazorPlate.Providers;

public class ReturnUrlProvider(ILocalStorageService localStorageService) : IReturnUrlProvider
{
    #region Private Fields

    private const string ReturnUrl = "ReturnUrl";

    #endregion Private Fields

    #region Public Methods

    public async Task<string> GetReturnUrl()
    {
        var returnUrl = await localStorageService.GetItemAsync<string>(ReturnUrl);
        return returnUrl ?? string.Empty;
    }

    public async Task SetReturnUrl(string returnUrl)
    {
        if (!returnUrl.Contains("401"))
            await localStorageService.SetItemAsync(ReturnUrl, returnUrl);
    }

    public async Task RemoveReturnUrl()
    {
        await localStorageService.RemoveItemAsync(ReturnUrl);
    }

    #endregion Public Methods
}