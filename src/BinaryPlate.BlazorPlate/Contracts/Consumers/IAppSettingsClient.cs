namespace BinaryPlate.BlazorPlate.Contracts.Consumers;

/// <summary>
/// Provides methods for interacting with the application settings related to identity, file storage
/// and tokens.
/// </summary>
public interface IAppSettingsClient
{
    #region Public Methods

    /// <summary>
    /// Gets the identity settings for editing.
    /// </summary>
    /// <returns>An <see cref="IdentitySettingsForEdit"/>.</returns>
    Task<ApiResponseWrapper<IdentitySettingsForEdit>> GetIdentitySettings();

    /// <summary>
    /// Updates the identity settings.
    /// </summary>
    /// <param name="request">The request containing the updated identity settings.</param>
    /// <returns>An <see cref="GetIdentitySettingsResponse"/>.</returns>
    Task<ApiResponseWrapper<GetIdentitySettingsResponse>> UpdateIdentitySettings(UpdateIdentitySettingsCommand request);

    /// <summary>
    /// Gets the file storage settings for editing.
    /// </summary>
    /// <returns>A <see cref="FileStorageSettingsForEdit"/>.</returns>
    Task<ApiResponseWrapper<FileStorageSettingsForEdit>> GetFileStorageSettings();

    /// <summary>
    /// Updates the file storage settings.
    /// </summary>
    /// <param name="request">The request containing the updated file storage settings.</param>
    /// <returns>A <see cref="GetFileStorageSettingsResponse"/>.</returns>
    Task<ApiResponseWrapper<GetFileStorageSettingsResponse>> UpdateFileStorageSettings(UpdateFileStorageSettingsCommand request);

    /// <summary>
    /// Gets the token settings for editing.
    /// </summary>
    /// <returns>A <see cref="TokenSettingsForEdit"/>.</returns>
    Task<ApiResponseWrapper<TokenSettingsForEdit>> GetTokenSettings();

    /// <summary>
    /// Updates the token settings.
    /// </summary>
    /// <param name="request">The request containing the updated token settings.</param>
    /// <returns>A <see cref="GetTokenSettingsResponse"/>.</returns>
    Task<ApiResponseWrapper<GetTokenSettingsResponse>> UpdateTokenSettings(UpdateTokenSettingsCommand request);

    #endregion Public Methods
}