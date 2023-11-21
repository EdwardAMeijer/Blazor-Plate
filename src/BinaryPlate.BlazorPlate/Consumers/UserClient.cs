namespace BinaryPlate.BlazorPlate.Consumers;

public class UsersClient(IHttpService httpService) : IUsersClient
{
    #region Public Methods

    public async Task<ApiResponseWrapper<UserForEdit>> GetUser(GetUserForEditQuery request)
    {
        return await httpService.Post<GetUserForEditQuery, UserForEdit>("identity/users/GetUser", request);
    }

    public async Task<ApiResponseWrapper<GetUsersResponse>> GetUsers(GetUsersQuery request)
    {
        return await httpService.Post<GetUsersQuery, GetUsersResponse>("identity/users/GetUsers", request);
    }

    public async Task<ApiResponseWrapper<CreateUserResponse>> CreateUser(CreateUserCommand request)
    {
        return await httpService.Post<CreateUserCommand, CreateUserResponse>("identity/users/CreateUser", request);
    }

    public async Task<ApiResponseWrapper<string>> UpdateUser(UpdateUserCommand request)
    {
        return await httpService.Put<UpdateUserCommand, string>("identity/users/UpdateUser", request);
    }

    public async Task<ApiResponseWrapper<string>> DeleteUser(string id)
    {
        return await httpService.Delete<string>($"identity/users/DeleteUser?id={id}");
    }

    public async Task<ApiResponseWrapper<GetUserPermissionsResponse>> GetUserPermissions(GetUserPermissionsQuery request)
    {
        return await httpService.Post<GetUserPermissionsQuery, GetUserPermissionsResponse>("identity/users/GetUserPermissions", request);
    }

    public async Task<ApiResponseWrapper<string>> GrantOrRevokeUserPermissions(GrantOrRevokeUserPermissionsCommand request)
    {
        return await httpService.Post<GrantOrRevokeUserPermissionsCommand, string>("identity/users/GrantOrRevokeUserPermissions", request);
    }

    #endregion Public Methods
}