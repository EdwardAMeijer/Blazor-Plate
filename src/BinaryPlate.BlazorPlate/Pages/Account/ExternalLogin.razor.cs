namespace BinaryPlate.BlazorPlate.Pages.Account
{
    public partial class ExternalLogin
    {
        #region Private Properties

        [Inject] private IAuthenticationService AuthenticationService { get; set; }
        [Inject] private IReturnUrlProvider ReturnUrlProvider { get; set; }
        [Inject] private IAppStateManager AppStateManager { get; set; }
        [Inject] private IAccountsClient AccountsClient { get; set; }
        [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
        [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }

        #endregion Private Properties

        #region Protected Methods

        protected override async Task OnInitializedAsync()
        {
            AppStateManager.OverlayVisible = true;

            var authState = await AuthStateProvider.GetAuthenticationStateAsync();

            var userIdentity = authState.User.Identity ?? throw new ArgumentNullException(nameof(authState.User));

            if (!userIdentity.IsAuthenticated)
            {
                var responseWrapper = await AccountsClient.RegisterExternalLogin();

                if (responseWrapper.IsSuccessStatusCode)
                    await AuthenticationService.Login(responseWrapper.Payload.AuthResponse);
                else
                    SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);

                AppStateManager.OverlayVisible = false;
            }

            var returnUrl = await ReturnUrlProvider.GetReturnUrl();

            await ReturnUrlProvider.RemoveReturnUrl();

            AppStateManager.OverlayVisible = false;

            NavigationManager.NavigateTo(returnUrl);
        }

        #endregion Protected Methods
    }
}