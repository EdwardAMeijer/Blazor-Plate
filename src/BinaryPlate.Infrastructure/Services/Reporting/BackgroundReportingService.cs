namespace BinaryPlate.Infrastructure.Services.Reporting;

public class BackgroundReportingService(IUtcDateTimeProvider utcDateTimeProvider,
                                        IApplicationDbContext dbContext,
                                        IHubNotificationService hubNotificationService,
                                        IReportDataProvider reportDataProvider,
                                        IHtmlReportBuilderService htmlReportBuilderService) : IBackgroundReportingService
{
    #region Public Methods

    public async Task<FileMetaData> ExportDataAsPdfInBackground(dynamic request, Guid reportId, string baseUrl, string userNameIdentifier)
    {
        // Create a new FileMetaData object.
        FileMetaData fileMetaData = new();

        try
        {
            // Update the report status to in progress and notify the user.
            await UpdateStatusAndNotify(reportId, ReportStatus.InProgress, userNameIdentifier, fileMetaData);

            var applicantsResponse = await reportDataProvider.GetApplicants(request);

            // Generate the PDF file from the provided HTML template and the applicant data.
            fileMetaData = await htmlReportBuilderService.GeneratePdfReportFromHtml(applicantsResponse.Payload.Applicants.Items, "applicants.html", baseUrl);

            // Update the report status to completed and notify the user.
            await UpdateStatusAndNotify(reportId, ReportStatus.Completed, userNameIdentifier, fileMetaData);
        }
        catch
        {
            // If an exception occurs, update the report status to failed and notify the user.
            await UpdateStatusAndNotify(reportId, ReportStatus.Failed, userNameIdentifier, fileMetaData);
        }

        // Return the FileMetaData object.
        return fileMetaData;
    }

    public async Task InitiateReport(dynamic request, Guid reportId, string userNameIdentifier)
    {
        // Update the report status to pending and notify the user.
        await UpdateStatusAndNotify(reportId, ReportStatus.Pending, userNameIdentifier);

        // Set the initial status for the report.
        await SetInitialReportStatus(request, reportId);
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Set an initial pending status for a report based on the provided query and report.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="reportId"></param>
    /// <returns></returns>
    private async Task SetInitialReportStatus(dynamic request, Guid reportId)
    {
        dbContext.Reports.Add(new Report
        {
            Id = reportId,
            Title = "N/A",
            QueryString = $"SearchText:{request.SearchText ?? "All"}, SortBy:{request.SortBy}",
            Status = (int)ReportStatus.Pending
        });

        await dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Update the status of a report and notify the issuer and viewers.
    /// </summary>
    /// <param name="reportId"></param>
    /// <param name="status"></param>
    /// <param name="userNameIdentifier"></param>
    /// <param name="fileMetaData"></param>
    /// <returns></returns>
    private async Task UpdateStatusAndNotify(Guid reportId, ReportStatus status, string userNameIdentifier, FileMetaData fileMetaData = null)
    {
        // Wait briefly to simulate processing time.
        //await Task.Delay(1000);

        // Retrieve the report from the database.
        var report = await dbContext.Reports.Where(r => r.Id == reportId).FirstOrDefaultAsync();

        // If the report exists, update its status and associated file metadata.
        if (report != null)
        {
            report.Title = $"{utcDateTimeProvider.GetUtcNow().ToLongDateString()} {fileMetaData?.FileName}";
            report.Status = (int)status;
            report.ContentType = fileMetaData?.ContentType;
            report.FileName = fileMetaData?.FileName;
            report.FileUri = fileMetaData?.FileUri;
            await dbContext.SaveChangesAsync();
            // Notify the issuer of the updated status.
            await hubNotificationService.NotifyReportIssuer(userNameIdentifier, fileMetaData, status);
        }

        await hubNotificationService.RefreshReportsViewer(userNameIdentifier);
    }

    #endregion Private Methods
}