namespace BinaryPlate.Application.Common.Contracts.Application;

/// <summary>
/// Represents a service for reading application settings.
/// </summary>
public interface IAppSettingsService
{
    #region Public Methods

    /// <summary>
    /// Retrieves the identity settings for editing.
    /// </summary>
    /// <returns>An envelope containing the identity settings response.</returns>
    Task<Envelope<GetIdentitySettingsForEditResponse>> GetIdentitySettings();

    /// <summary>
    /// Retrieves the token settings for editing.
    /// </summary>
    /// <returns>An envelope containing the token settings response.</returns>
    Task<Envelope<GetTokenSettingsForEditResponse>> GetTokenSettings();

    /// <summary>
    /// Retrieves the file storage settings for editing.
    /// </summary>
    /// <returns>An envelope containing the file storage settings response.</returns>
    Task<Envelope<GetFileStorageSettingsForEditResponse>> GetFileStorageSettings();

    #endregion Public Methods
}