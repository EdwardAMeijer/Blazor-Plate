namespace BinaryPlate.BlazorPlate.Providers;

public class SnackbarApiExceptionProvider(ISnackbar snackbar)
{
    #region Public Methods

    public void ShowErrors(ApiErrorResponse apiErrorResponse)
    {
        if (!string.IsNullOrEmpty(apiErrorResponse.Title))
            snackbar.Add(apiErrorResponse.Title, Severity.Error, (options) => options.CloseAfterNavigation = true);

        if (!string.IsNullOrEmpty(apiErrorResponse.ErrorMessage))
            snackbar.Add(apiErrorResponse.ErrorMessage, Severity.Error, (options) => options.CloseAfterNavigation = true);

        if (!string.IsNullOrEmpty(apiErrorResponse.InnerException))
            snackbar.Add(apiErrorResponse.InnerException, Severity.Error, (options) => options.CloseAfterNavigation = true);

        if (apiErrorResponse.ValidationErrors != null && apiErrorResponse.ValidationErrors.Any())
            foreach (var error in apiErrorResponse.ValidationErrors)
                snackbar.Add(error.Reason, Severity.Error, (options) => { options.CloseAfterNavigation = false; });
    }

    #endregion Public Methods
}