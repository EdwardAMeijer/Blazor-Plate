namespace BinaryPlate.BlazorPlate.Pages.Account.Manage;

public partial class DangerZone
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private IManageClient ManageClient { get; set; }
    [Inject] private IJSRuntime JsRuntime { get; set; }

    #endregion Private Properties

    #region Private Methods

    private async Task DownloadPersonalData()
    {
        var responseWrapper = await ManageClient.DownloadPersonalData();

        if (responseWrapper.IsSuccessStatusCode)
            await JsRuntime.InvokeVoidAsync("blazorDownloadFile", responseWrapper.Payload.FileName, responseWrapper.Payload.ContentType, Convert.ToBase64String(responseWrapper.Payload.PersonalData));
        else
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
    }

    private void DeleteAccount()
    {
        DialogService.Show<DeleteAccount>();
    }

    #endregion Private Methods
}