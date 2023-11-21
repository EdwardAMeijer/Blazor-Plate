namespace BinaryPlate.BlazorPlate.Pages.Settings;

public partial class IdentitySettings
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IAppSettingsClient AppSettingsClient { get; set; }

    private EditContextApiExceptionFallback EditContextApiExceptionFallback { get; set; }
    private IdentitySettingsForEdit IdentitySettingsForEditVm { get; set; } = new();
    private UpdateIdentitySettingsCommand UpdateIdentitySettingsCommand { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override async Task OnInitializedAsync()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Settings, "#",true),
            new(Resource.Identity_Settings, "#",true)
        });

        var responseWrapper = await AppSettingsClient.GetIdentitySettings();

        if (responseWrapper.IsSuccessStatusCode)
        {
            var identitySettingsForEdit = responseWrapper.Payload;
            IdentitySettingsForEditVm = identitySettingsForEdit;
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task SubmitForm()
    {
        var dialog = await DialogService.ShowAsync<SaveConfirmationDialog>(Resource.Confirm);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            UpdateIdentitySettingsCommand = new UpdateIdentitySettingsCommand
            {
                UserSettingsModel = IdentitySettingsForEditVm.UserSettingsForEdit.MapToCommand(),
                LockoutSettingsModel = IdentitySettingsForEditVm.LockoutSettingsForEdit.MapToCommand(),
                PasswordSettingsModel = IdentitySettingsForEditVm.PasswordSettingsForEdit.MapToCommand(),
                SignInSettingsModel = IdentitySettingsForEditVm.SignInSettingsForEdit.MapToCommand(),
            };

            var responseWrapper = await AppSettingsClient.UpdateIdentitySettings(UpdateIdentitySettingsCommand);

            if (responseWrapper.IsSuccessStatusCode)
            {
                IdentitySettingsForEditVm.UserSettingsForEdit.Id = responseWrapper.Payload.UserSettingsId;
                IdentitySettingsForEditVm.LockoutSettingsForEdit.Id = responseWrapper.Payload.LockoutSettingsId;
                IdentitySettingsForEditVm.PasswordSettingsForEdit.Id = responseWrapper.Payload.PasswordSettingsId;
                IdentitySettingsForEditVm.SignInSettingsForEdit.Id = responseWrapper.Payload.SignInSettingsId;

                IdentitySettingsForEditVm.UserSettingsForEdit.ConcurrencyStamp = responseWrapper.Payload.UserSettingsConcurrencyStamp;
                IdentitySettingsForEditVm.LockoutSettingsForEdit.ConcurrencyStamp = responseWrapper.Payload.LockoutSettingsConcurrencyStamp;
                IdentitySettingsForEditVm.PasswordSettingsForEdit.ConcurrencyStamp = responseWrapper.Payload.PasswordSettingsConcurrencyStamp;
                IdentitySettingsForEditVm.SignInSettingsForEdit.ConcurrencyStamp = responseWrapper.Payload.SignInSettingsConcurrencyStamp;

                Snackbar.Add(responseWrapper.Payload.SuccessMessage, Severity.Success);
            }
            else
            {
                EditContextApiExceptionFallback.PopulateFormErrors(responseWrapper.ApiErrorResponse);
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            }
        }
    }

    #endregion Private Methods
}