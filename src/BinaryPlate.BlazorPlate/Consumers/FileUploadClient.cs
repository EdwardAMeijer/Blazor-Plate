namespace BinaryPlate.BlazorPlate.Consumers;

public class FileUploadClient(IHttpService httpService) : IFileUploadClient
{
    #region Public Methods

    public async Task<ApiResponseWrapper<FileUploadResponse>> UploadFile(MultipartFormDataContent request)
    {
        return await httpService.PostFormData<MultipartFormDataContent, FileUploadResponse>("fileUpload/uploadFile", request);
    }

    #endregion Public Methods
}