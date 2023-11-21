namespace BinaryPlate.Application.Common.Contracts.Infrastructure.Reporting;

/// <summary>
/// Exports application-specific reports on demand.
/// </summary>
public interface IOnDemandReportingService
{
    #region Public Methods

    /// <summary>
    /// Export applicants as a PDF file on-demand.
    /// </summary>
    /// <param name="data">Dynamic object containing report data.</param>
    /// <param name="baseUrl">The base URL for generating URLs in the PDF.</param>
    /// <param name="issuerName">The name of the requester or issuer.</param>
    /// <returns>A dynamic object representing the exported PDF file with properties like ContentType, FileName, and FileUri.</returns>
    Task<dynamic> ExportDataAsPdfOnDemand(dynamic data, string baseUrl, string issuerName);

    #endregion Public Methods
}