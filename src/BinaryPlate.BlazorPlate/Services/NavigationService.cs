namespace BinaryPlate.BlazorPlate.Services;

public class NavigationService(IJSRuntime jsRuntime) : INavigationService
{
    #region Public Methods

    public async Task NavigateToUrlAsync(string url, bool openInNewTab)
    {
        if (openInNewTab)
            await jsRuntime.InvokeVoidAsync("open", url, "_blank");
        else
            await jsRuntime.InvokeVoidAsync("open", url);
    }

    #endregion Public Methods
}