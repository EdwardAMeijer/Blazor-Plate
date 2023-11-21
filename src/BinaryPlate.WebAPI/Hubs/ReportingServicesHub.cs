namespace BinaryPlate.WebAPI.Hubs;

[Authorize]
public class ReportingServicesHub(IBackgroundJobClient backgroundJob,
                                  IHttpContextAccessor httpContextAccessor,
                                  ISignalRContextProvider signalRContextProvider,
                                  IBackgroundReportingService backgroundReportingService) : Hub
{
    #region Private Fields

    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    #endregion Private Fields

    #region Public Methods

    public async Task ExportApplicantToPdf(ExportApplicantsQuery request)
    {
        // Get the unique identifier of the user who initiated the request.
        var userNameIdentifier = signalRContextProvider.GetUserNameIdentifier(Context);

        // Get the host name associated with the context.
        var baseUrl = signalRContextProvider.GetHostName(Context);

        // Generate a unique report id.
        var reportId = Guid.NewGuid();

        // TODO: Uncomment the following code block if Hangfire is not being used.
        //await _backgroundReportingService.InitiateReport(request, reportId, userNameIdentifier);
        //await _backgroundReportingService.ExportDataAsPdfInBackground(request, reportId, host, issuerName, userNameIdentifier);

        var pendingJob = backgroundJob.Enqueue(() => backgroundReportingService.InitiateReport(request, reportId, userNameIdentifier));

        // Add a record to the Reports Table with an in-progress status.
        backgroundJob.ContinueJobWith(pendingJob, () => backgroundReportingService.ExportDataAsPdfInBackground(request, reportId, baseUrl, userNameIdentifier));

        await Task.CompletedTask;
    }

    public override Task OnConnectedAsync()
    {
        if (Context.UserIdentifier != null)
        {
            // Get the name of the user who connected to the hub.
            var name = signalRContextProvider.GetUserName(Context);

            // Add the user to the group associated with their name.
            Groups.AddToGroupAsync(Context.ConnectionId, name);
        }

        return base.OnConnectedAsync();
    }

    #endregion Public Methods
}