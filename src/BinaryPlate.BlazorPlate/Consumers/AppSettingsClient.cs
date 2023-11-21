namespace BinaryPlate.BlazorPlate.Consumers;

public class AppSettingsClient(IHttpService httpService) : IAppSettingsClient
{
    #region Public Methods

    public async Task<ApiResponseWrapper<IdentitySettingsForEdit>> GetIdentitySettings()
    {
        return await httpService.Get<IdentitySettingsForEdit>("AppSettings/GetIdentitySettings");
    }

    public async Task<ApiResponseWrapper<GetIdentitySettingsResponse>> UpdateIdentitySettings(UpdateIdentitySettingsCommand request)
    {
        return await httpService.Put<UpdateIdentitySettingsCommand, GetIdentitySettingsResponse>("AppSettings/UpdateIdentitySettings", request);
    }

    public async Task<ApiResponseWrapper<FileStorageSettingsForEdit>> GetFileStorageSettings()
    {
        return await httpService.Get<FileStorageSettingsForEdit>("AppSettings/GetFileStorageSettings");
    }

    public async Task<ApiResponseWrapper<GetFileStorageSettingsResponse>> UpdateFileStorageSettings(UpdateFileStorageSettingsCommand request)
    {
        return await httpService.Put<UpdateFileStorageSettingsCommand, GetFileStorageSettingsResponse>("AppSettings/UpdateFileStorageSettings", request);
    }

    public async Task<ApiResponseWrapper<TokenSettingsForEdit>> GetTokenSettings()
    {
        return await httpService.Get<TokenSettingsForEdit>("AppSettings/GetTokenSettings");
    }

    public async Task<ApiResponseWrapper<GetTokenSettingsResponse>> UpdateTokenSettings(UpdateTokenSettingsCommand request)
    {
        return await httpService.Put<UpdateTokenSettingsCommand, GetTokenSettingsResponse>("AppSettings/UpdateTokenSettings", request);
    }

    #endregion Public Methods
}