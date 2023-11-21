namespace BinaryPlate.Application.Features.AppSettings.Queries.GetSettings.GetFileStorageSettings;

public class GetFileStorageSettingsForEditResponse
{
    #region Public Properties

    public string Id { get; set; }
    public int StorageType { get; set; }
    public string ConcurrencyStamp { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static GetFileStorageSettingsForEditResponse MapFromEntity(FileStorageSettings fileStorageSettings)
    {
        return new()
        {
            Id = fileStorageSettings.Id.ToString(),
            StorageType = (int)fileStorageSettings.StorageType,
            ConcurrencyStamp = fileStorageSettings.ConcurrencyStamp
        };
    }

    #endregion Public Methods
}