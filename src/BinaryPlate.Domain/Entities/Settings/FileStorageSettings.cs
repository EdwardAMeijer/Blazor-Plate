namespace BinaryPlate.Domain.Entities.Settings;

/// <summary>
/// Represents the settings for file storage.
/// </summary>
public class FileStorageSettings : ISettingsSchema, IConcurrencyStamp
{
    #region Public Properties

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the storage type used for file storage.
    /// </summary>
    public StorageTypes StorageType { get; set; }

    public string ConcurrencyStamp { get; set; }


    #endregion Public Properties
}