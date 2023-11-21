namespace BinaryPlate.BlazorPlate.Contracts.Consumers;

/// <summary>
/// Provides methods for managing permissions.
/// </summary>
public interface IPermissionsClient
{
    #region Public Methods

    /// <summary>
    /// Gets the list of permissions.
    /// </summary>
    /// <param name="request">The query to filter the list of permissions.</param>
    /// <returns>A <see cref="GetPermissionsResponse"/>.</returns>
    Task<ApiResponseWrapper<GetPermissionsResponse>> GetPermissions(GetPermissionsQuery request);

    #endregion Public Methods
}