namespace BinaryPlate.BlazorPlate.Consumers;

public class RolesClient(IHttpService httpService) : IRolesClient
{
    #region Public Methods

    public async Task<ApiResponseWrapper<RoleForEdit>> GetRole(GetRoleForEditQuery request)
    {
        return await httpService.Post<GetRoleForEditQuery, RoleForEdit>("identity/roles/GetRole", request);
    }

    public async Task<ApiResponseWrapper<GetRolePermissionsResponse>> GetRolePermissions(GetRolePermissionsForEditQuery request)
    {
        return await httpService.Post<GetRolePermissionsForEditQuery, GetRolePermissionsResponse>("identity/roles/GetRolePermissions", request);
    }

    public async Task<ApiResponseWrapper<GetRolesResponse>> GetRoles(GetRolesQuery request)
    {
        return await httpService.Post<GetRolesQuery, GetRolesResponse>("identity/roles/GetRoles", request);
    }

    public async Task<ApiResponseWrapper<CreateRoleResponse>> CreateRole(CreateRoleCommand request)
    {
        return await httpService.Post<CreateRoleCommand, CreateRoleResponse>("identity/roles/CreateRole", request);
    }

    public async Task<ApiResponseWrapper<string>> UpdateRole(UpdateRoleCommand request)
    {
        return await httpService.Put<UpdateRoleCommand, string>("identity/roles/UpdateRole", request);
    }

    public async Task<ApiResponseWrapper<string>> DeleteRole(string id)
    {
        return await httpService.Delete<string>($"identity/roles/DeleteRole?id={id}");
    }

    #endregion Public Methods
}