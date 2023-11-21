namespace BinaryPlate.BlazorPlate.Models;

public class FileUploadMetaData
{
    #region Public Properties

    public string Name { get; set; }
    public string Type { get; set; }
    public string Size { get; set; }
    public MultipartFormDataContent Content { get; set; } = new();

    #endregion Public Properties
}