namespace BinaryPlate.BlazorPlate.Contracts.Consumers;

/// <summary>
/// Provides methods for viewing dashboard data.
/// </summary>
public interface IDashboardClient
{
    #region Public Methods

    /// <summary>
    /// Gets the headlines data.
    /// </summary>
    /// <returns>A <see cref="GetHeadlinesResponse"/>.</returns>
    Task<ApiResponseWrapper<GetHeadlinesResponse>> GetHeadlinesData();

    #endregion Public Methods
}