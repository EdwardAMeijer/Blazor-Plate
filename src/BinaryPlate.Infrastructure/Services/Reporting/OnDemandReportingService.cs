namespace BinaryPlate.Infrastructure.Services.Reporting;

public class OnDemandReportingService(IHtmlReportBuilderService htmlReportBuilderService) : IOnDemandReportingService
{
    #region Public Methods

    public async Task<dynamic> ExportDataAsPdfOnDemand(dynamic data, string issuerName, string baseUrl = null)
    {
        // Create an ExpandoObject to store the dynamic response properties.
        dynamic response = new ExpandoObject();

        // Generate the PDF file by converting provided HTML template and data.
        var fileMetaData = await htmlReportBuilderService.GeneratePdfReportFromHtml(data.Applicants.Items, "applicants.html", baseUrl);

        // Set the response properties on the dynamic object.
        response.ContentType = fileMetaData.ContentType as string;
        response.FileName = fileMetaData.FileName as string;
        response.FileUri = fileMetaData.FileUri as string;

        // Return the dynamic response object.
        return response;
    }


    #endregion Public Methods
}