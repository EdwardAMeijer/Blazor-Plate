namespace BinaryPlate.WebAPI.Services.HubServices;

public class HubNotificationService(IHubContext<ReportingServicesHub> hubContext) : IHubNotificationService
{
    #region Public Methods

    public async Task NotifyReportIssuer(string userNameIdentifier, FileMetaData fileMetaData, ReportStatus status)
    {
        // Send a notification to the specified user with the given user name identifier, passing in
        // the file metadata and report status.
        await hubContext.Clients.User(userNameIdentifier).SendAsync("NotifyReportIssuer", fileMetaData, status);

        // TODO: Uncomment the following line to send the notification to all clients connected to the hub.
        //await _hubContext.Clients.All.SendAsync("NotifyReportSubscriber", $"Hi=>{userName}");
    }

    public async Task RefreshReportsViewer(string userNameIdentifier)
    {
        // Send a message to the specified user with the given user name identifier to refresh the
        // reports viewer.
        await hubContext.Clients.User(userNameIdentifier).SendAsync("RefreshReportsViewer");
    }

    #endregion Public Methods
}