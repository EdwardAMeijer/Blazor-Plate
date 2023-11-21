namespace BinaryPlate.Application.Features.Reports.GetReports;

public class GetReportsResponse
{
    #region Public Properties

    public PagedList<ReportItem> Reports { get; set; }

    #endregion Public Properties
}