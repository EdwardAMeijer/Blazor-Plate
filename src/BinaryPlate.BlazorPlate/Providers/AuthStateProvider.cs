namespace BinaryPlate.BlazorPlate.Providers;

public class AuthStateProvider(IAccessTokenProvider accessTokenProvider) : AuthenticationStateProvider
{
    #region Private Fields

    private readonly AuthenticationState _anonymous = new(new ClaimsPrincipal(new ClaimsIdentity()));

    #endregion Private Fields

    #region Public Methods

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await accessTokenProvider.TryGetAccessToken();

        if (string.IsNullOrWhiteSpace(token))
            return _anonymous;

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType")));
    }

    public void NotifyUserAuthentication(string token)
    {
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaimsFromJwt(token), "jwtAuthType"));

        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));

        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        var authState = Task.FromResult(_anonymous);

        NotifyAuthenticationStateChanged(authState);
    }

    #endregion Public Methods
}