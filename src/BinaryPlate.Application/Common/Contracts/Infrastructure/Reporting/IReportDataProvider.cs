namespace BinaryPlate.Application.Common.Contracts.Infrastructure.Reporting;

/// <summary>
/// Represents a service for fetching report data.
/// </summary>
public interface IReportDataProvider
{
    #region Public Methods

    /// <summary>
    /// Retrieves a list of applicants based on the specified query.
    /// </summary>
    /// <param name="request">The query specifying the applicants to retrieve.</param>
    /// <returns>An envelope containing the applicants response.</returns>
    Task<Envelope<dynamic>> GetApplicants(dynamic request);

    #endregion Public Methods
}