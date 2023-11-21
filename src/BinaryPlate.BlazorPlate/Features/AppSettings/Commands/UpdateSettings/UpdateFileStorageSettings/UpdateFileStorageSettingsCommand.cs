namespace BinaryPlate.BlazorPlate.Features.AppSettings.Commands.UpdateSettings.UpdateFileStorageSettings;

public class UpdateFileStorageSettingsCommand
{
    #region Public Properties

    public string Id { get; set; }
    public int StorageType { get; set; }
    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties
}