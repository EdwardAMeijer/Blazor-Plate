namespace BinaryPlate.BlazorPlate.Consumers;

public class ManageClient(IHttpService httpService) : IManageClient
{
    #region Public Methods

    public async Task<ApiResponseWrapper<CurrentUserForEdit>> GetUser()
    {
        return await httpService.Get<CurrentUserForEdit>("account/manage/GetCurrentUser");
    }

    public async Task<ApiResponseWrapper<UserAvatarForEdit>> GetUserAvatar()
    {
        return await httpService.Get<UserAvatarForEdit>("account/manage/GetUserAvatar");
    }

    public async Task<ApiResponseWrapper<string>> UpdateUserProfile(UpdateUserProfileCommand request)
    {
        return await httpService.Put<UpdateUserProfileCommand, string>("account/manage/UpdateUserProfile", request);
    }

    public async Task<ApiResponseWrapper<string>> UpdateUserAvatar(UpdateUserAvatarCommand request)
    {
        return await httpService.Put<UpdateUserAvatarCommand, string>("account/manage/UpdateUserAvatar", request);
    }

    public async Task<ApiResponseWrapper<ChangeEmailResponse>> ChangeEmail(ChangeEmailCommand request)
    {
        return await httpService.Post<ChangeEmailCommand, ChangeEmailResponse>("account/manage/ChangeEmail", request);
    }

    public async Task<ApiResponseWrapper<ChangeEmailResponse>> ConfirmEmailChange(ConfirmEmailChangeCommand request)
    {
        return await httpService.Put<ConfirmEmailChangeCommand, ChangeEmailResponse>("account/manage/ConfirmEmailChange", request);
    }

    public async Task<ApiResponseWrapper<ChangePasswordResponse>> ChangePassword(ChangePasswordCommand request)
    {
        return await httpService.Post<ChangePasswordCommand, ChangePasswordResponse>("account/manage/ChangePassword", request);
    }

    public async Task<ApiResponseWrapper<SetPasswordResponse>> SetPassword(SetPasswordCommand request)
    {
        return await httpService.Post<SetPasswordCommand, SetPasswordResponse>("account/manage/SetPassword", request);
    }

    public async Task<ApiResponseWrapper<GetTwoFactorAuthenticationStateResponse>> Get2FaState()
    {
        return await httpService.Get<GetTwoFactorAuthenticationStateResponse>("account/manage/Get2FaState");
    }

    public async Task<ApiResponseWrapper<LoadSharedKeyAndQrCodeUriResponse>> LoadSharedKeyAndQrCodeUri()
    {
        return await httpService.Get<LoadSharedKeyAndQrCodeUriResponse>("account/manage/LoadSharedKeyAndQrCodeUri");
    }

    public async Task<ApiResponseWrapper<EnableAuthenticatorResponse>> EnableAuthenticator(EnableAuthenticatorCommand request)
    {
        return await httpService.Post<EnableAuthenticatorCommand, EnableAuthenticatorResponse>("account/manage/EnableAuthenticator", request);
    }

    public async Task<ApiResponseWrapper<string>> Disable2Fa()
    {
        return await httpService.Post<string>("account/manage/Disable2Fa");
    }

    public async Task<ApiResponseWrapper<GenerateRecoveryCodesResponse>> GenerateRecoveryCodes()
    {
        return await httpService.Get<GenerateRecoveryCodesResponse>("account/manage/GenerateRecoveryCodes");
    }

    public async Task<ApiResponseWrapper<GetUser2FaStateResponse>> CheckUser2FaState()
    {
        return await httpService.Get<GetUser2FaStateResponse>("account/manage/CheckUser2FaState");
    }

    public async Task<ApiResponseWrapper<ResetAuthenticatorResponse>> ResetAuthenticator()
    {
        return await httpService.Post<ResetAuthenticatorResponse>("account/manage/ResetAuthenticator");
    }

    public async Task<ApiResponseWrapper<DownloadPersonalDataResponse>> DownloadPersonalData()
    {
        return await httpService.Get<DownloadPersonalDataResponse>("account/manage/DownloadPersonalData");
    }

    public async Task<ApiResponseWrapper<string>> DeleteAccount(DeleteAccountCommand request)
    {
        return await httpService.Post<DeleteAccountCommand, string>("account/manage/DeleteAccount", request);
    }

    public async Task<ApiResponseWrapper<bool>> RequirePassword()
    {
        return await httpService.Get<bool>("account/manage/RequirePassword");
    }

    #endregion Public Methods
}