namespace BinaryPlate.BlazorPlate.Consumers;

public class ReportsClient(IHttpService httpService) : IReportsClient
{
    #region Public Methods

    public async Task<ApiResponseWrapper<ReportForEdit>> GetReport(GetReportForEditQuery request)
    {
        return await httpService.Post<GetReportForEditQuery, ReportForEdit>("reports/GetReport", request);
    }

    public async Task<ApiResponseWrapper<GetReportsResponse>> GetReports(GetReportsQuery request)
    {
        return await httpService.Post<GetReportsQuery, GetReportsResponse>("reports/GetReports", request);
    }

    #endregion Public Methods
}