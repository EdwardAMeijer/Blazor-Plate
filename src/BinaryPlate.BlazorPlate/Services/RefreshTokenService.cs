namespace BinaryPlate.BlazorPlate.Services;

public class RefreshTokenService(ILocalStorageService localStorageService,
                                 IAuthenticationService authenticationService,
                                 IAccountsClient accountsClient,
                                 AuthStateProvider authStateProvider,
                                 IHttpClientFactory httpClientFactory) : IRefreshTokenService
{
    #region Public Methods

    public async Task<string> TryRefreshToken()
    {
        var authState = await authStateProvider.GetAuthenticationStateAsync();

        var user = authState.User;

        if (user.Identity is { IsAuthenticated: false })
            return string.Empty;

        var accessTokenExpiryDate = user.FindFirst(c => c.Type.Equals("exp"))?.Value;

        var refreshAt = user.FindFirst(c => c.Type.Equals("refreshAt"))?.Value;

        if (refreshAt == null)
            return string.Empty;

        var refreshTokenExpiryDate = long.Parse(refreshAt);

        var utcNowUnix = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
        if (accessTokenExpiryDate != null && long.Parse(accessTokenExpiryDate) <= utcNowUnix && refreshTokenExpiryDate > utcNowUnix)
            return await RefreshToken(utcNowUnix, refreshTokenExpiryDate);

        if (refreshTokenExpiryDate <= utcNowUnix)
            await authenticationService.Logout();

        return string.Empty;
    }

    public async Task<string> RefreshToken(long utcNowUnix, long refreshTokenExpiryDate)
    {
        var token = await localStorageService.GetItemAsync<string>(TokenType.AccessToken);

        var refreshToken = await localStorageService.GetItemAsync<string>(TokenType.RefreshToken);

        var responseWrapper = await accountsClient.RefreshToken(new RefreshTokenCommand { AccessToken = token, RefreshToken = refreshToken });

        if (responseWrapper.IsSuccessStatusCode)
        {
            await localStorageService.SetItemAsync(TokenType.AccessToken, responseWrapper.Payload.AccessToken);
            await localStorageService.SetItemAsync(TokenType.RefreshToken, responseWrapper.Payload.RefreshToken);
            using var httpClient = httpClientFactory.CreateClient("DefaultClient");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", responseWrapper.Payload.AccessToken);
            return responseWrapper.Payload.AccessToken;
        }

        if (refreshTokenExpiryDate > utcNowUnix)
            return token;

        await authenticationService.Logout();

        return string.Empty;
    }

    #endregion Public Methods
}