namespace BinaryPlate.BlazorPlate.Consumers;

public class PermissionsClient(IHttpService httpService) : IPermissionsClient
{
    #region Public Methods

    public async Task<ApiResponseWrapper<GetPermissionsResponse>> GetPermissions(GetPermissionsQuery request)
    {
        return await httpService.Post<GetPermissionsQuery, GetPermissionsResponse>("permissions/GetPermissions", request);
    }

    #endregion Public Methods
}