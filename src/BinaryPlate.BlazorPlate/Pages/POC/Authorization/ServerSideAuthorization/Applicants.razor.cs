﻿namespace BinaryPlate.BlazorPlate.Pages.POC.Authorization.ServerSideAuthorization;

public partial class Applicants
{
    #region Private Properties

    [Inject] private SnackbarApiExceptionProvider SnackbarApiExceptionProvider { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private IBreadcrumbService BreadcrumbService { get; set; }
    [Inject] private IApplicantsClient ApplicantsClient { get; set; }

    private string SearchString { get; set; }
    private MudTable<ApplicantItem> Table { get; set; }
    private GetApplicantsQuery GetApplicantsQuery { get; } = new();
    private GetApplicantsResponse GetApplicantsResponse { get; set; }

    #endregion Private Properties

    #region Protected Methods

    protected override void OnInitialized()
    {
        BreadcrumbService.SetBreadcrumbItems(new List<BreadcrumbItem>
        {
            new(Resource.Home, "/"),
            new(Resource.Proof_of_Concepts, "#", true),
            new(Resource.Authorization, "#", true),
            new(Resource.Server_Side_Authorization, "#", true),
            new(Resource.Applicants, "#",true)
        });
    }

    #endregion Protected Methods

    #region Private Methods

    private async Task<TableData<ApplicantItem>> ServerReload(TableState state)
    {
        GetApplicantsQuery.SearchText = SearchString;

        GetApplicantsQuery.PageNumber = state.Page + 1;

        GetApplicantsQuery.RowsPerPage = state.PageSize;

        GetApplicantsQuery.SortBy = state.SortDirection == SortDirection.None ? string.Empty : $"{state.SortLabel} {state.SortDirection}";

        var responseWrapper = await ApplicantsClient.GetApplicants(GetApplicantsQuery);

        var tableData = new TableData<ApplicantItem>();

        if (responseWrapper.IsSuccessStatusCode)
        {
            GetApplicantsResponse = responseWrapper.Payload;
            tableData = new TableData<ApplicantItem> { TotalItems = GetApplicantsResponse.Applicants.TotalRows, Items = GetApplicantsResponse.Applicants.Items };
        }
        else
        {
            SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
        }
        return tableData;
    }

    private void FilterApplicants(string searchString)
    {
        if (GetApplicantsResponse is null)
            return;
        SearchString = searchString;
        Table.ReloadServerData();
    }

    private void AddApplicant()
    {
        NavigationManager.NavigateTo("poc/authorization/serverSideAuthorization/AddApplicant");
    }

    private void ViewApplicant(string id)
    {
        NavigationManager.NavigateTo($"poc/authorization/serverSideAuthorization/ViewApplicant/{id}");
    }

    private void EditApplicant(string id)
    {
        NavigationManager.NavigateTo($"poc/authorization/serverSideAuthorization/EditApplicant/{id}");
    }

    private async Task DeleteApplicant(string id)
    {
        var dialog = await DialogService.ShowAsync<DeleteConfirmationDialog>(Resource.Delete);

        var dialogResult = await dialog.Result;

        if (!dialogResult.Canceled)
        {
            var responseWrapper = await ApplicantsClient.DeleteApplicant(id);

            if (responseWrapper.IsSuccessStatusCode)
            {
                Snackbar.Add(responseWrapper.Payload, Severity.Success);
                await Table.ReloadServerData();
            }
            else
            {
                SnackbarApiExceptionProvider.ShowErrors(responseWrapper.ApiErrorResponse);
            }
        }
    }

    #endregion Private Methods
}