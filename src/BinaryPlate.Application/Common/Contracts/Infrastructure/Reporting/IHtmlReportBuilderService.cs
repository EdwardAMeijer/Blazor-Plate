namespace BinaryPlate.Application.Common.Contracts.Infrastructure.Reporting;

/// <summary>
/// Generates HTML application reports.
/// </summary>
public interface IHtmlReportBuilderService
{
    #region Public Methods

    /// <summary>
    /// Generates a PDF report from HTML content and applicant data.
    /// </summary>
    /// <param name="applicantItems">A list of dynamic objects representing applicant data.</param>
    /// <param name="htmlTemplateName">The name of the HTML template to use for the report.</param>
    /// <param name="baseUrl">The base URL for resolving relative paths in the HTML template.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation. The result is a <see cref="FileMetaData"/> object containing information about the generated PDF file.</returns>
    Task<FileMetaData> GeneratePdfReportFromHtml(IReadOnlyList<dynamic> applicantItems, string htmlTemplateName, string baseUrl);

    #endregion Public Methods
}