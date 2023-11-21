namespace BinaryPlate.BlazorPlate.Features.AppSettings.Queries.GetSettings.GetFileStorageSettings;

public class FileStorageSettingsForEdit
{
    #region Public Properties

    public string Id { get; set; }
    public int StorageType { get; set; }
    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties
}