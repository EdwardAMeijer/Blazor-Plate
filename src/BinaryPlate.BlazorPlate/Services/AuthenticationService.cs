namespace BinaryPlate.BlazorPlate.Services;

public class AuthenticationService(AuthenticationStateProvider authStateProvider, ILocalStorageService localStorageService) : IAuthenticationService
{
    #region Public Methods

    public async Task Login(AuthResponse authResponse)
    {
        await Logout();

        await SetTokens(authResponse);
        ((AuthStateProvider)authStateProvider).NotifyUserAuthentication(authResponse.AccessToken);
    }

    public async Task ReAuthenticate(AuthResponse authResponse)
    {
        await SetTokens(authResponse);

        ((AuthStateProvider)authStateProvider).NotifyUserAuthentication(authResponse.AccessToken);
    }

    public async Task Logout()
    {
        var authState = await ((AuthStateProvider)authStateProvider).GetAuthenticationStateAsync();

        var user = authState.User;

        if (user.Identity is { IsAuthenticated: true })
        {
            await ClearTokens();
            ((AuthStateProvider)authStateProvider).NotifyUserLogout();
        }
    }

    #endregion Public Methods

    #region Private Methods

    private async Task SetTokens(AuthResponse authResponse)
    {
        await localStorageService.RemoveItemAsync(TokenType.AccessToken);

        await localStorageService.RemoveItemAsync(TokenType.RefreshToken);

        await localStorageService.SetItemAsync(TokenType.AccessToken, authResponse.AccessToken);

        await localStorageService.SetItemAsync(TokenType.RefreshToken, authResponse.RefreshToken);
    }

    private async Task ClearTokens()
    {
        await localStorageService.RemoveItemAsync(TokenType.AccessToken);

        await localStorageService.RemoveItemAsync(TokenType.RefreshToken);
    }

    #endregion Private Methods
}