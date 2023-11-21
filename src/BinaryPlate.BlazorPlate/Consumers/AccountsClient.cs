namespace BinaryPlate.BlazorPlate.Consumers;

public class AccountsClient(IHttpService httpService) : IAccountsClient
{
    #region Public Methods

    public async Task<ApiResponseWrapper<RegisterResponse>> Register(RegisterCommand request)
    {
        return await httpService.Post<RegisterCommand, RegisterResponse>("account/Register", request);
    }

    public async Task<ApiResponseWrapper<LoginResponse>> RegisterExternalLogin()
    {
        return await httpService.Get<LoginResponse>("account/ExternalLogins/RegisterExternalLogin", defaultClient: HttpClientType.ExternalAuthClient);
    }

    public async Task<ApiResponseWrapper<LoginResponse>> Login(LoginCommand request)
    {
        return await httpService.Post<LoginCommand, LoginResponse>("account/Login", request);
    }

    public async Task<ApiResponseWrapper<string>> Logout()
    {
        return await httpService.Post<string>("account/Logout");
    }

    public async Task<ApiResponseWrapper<LoginWith2FaResponse>> LoginWith2Fa(LoginWith2FaCommand request)
    {
        return await httpService.Post<LoginWith2FaCommand, LoginWith2FaResponse>("account/LoginWith2Fa", request);
    }

    public async Task<ApiResponseWrapper<LoginWithRecoveryCodeResponse>> LoginWithRecoveryCode(LoginWithRecoveryCodeCommand request)
    {
        return await httpService.Post<LoginWithRecoveryCodeCommand, LoginWithRecoveryCodeResponse>("account/LoginWithRecoveryCode", request);
    }

    public async Task<ApiResponseWrapper<ForgetPasswordResponse>> ForgetPassword(ForgetPasswordCommand request)
    {
        return await httpService.Post<ForgetPasswordCommand, ForgetPasswordResponse>("account/ForgotPassword", request);
    }

    public async Task<ApiResponseWrapper<string>> ResetPassword(ResetPasswordCommand request)
    {
        return await httpService.Post<ResetPasswordCommand, string>("account/ResetPassword", request);
    }

    public async Task<ApiResponseWrapper<string>> ConfirmEmail(ConfirmEmailCommand request)
    {
        return await httpService.Post<ConfirmEmailCommand, string>("account/ConfirmEmail", request);
    }

    public async Task<ApiResponseWrapper<ResendEmailConfirmationResponse>> ResendEmailConfirmation(ResendEmailConfirmationCommand request)
    {
        return await httpService.Post<ResendEmailConfirmationCommand, ResendEmailConfirmationResponse>("account/ResendEmailConfirmation", request);
    }

    public async Task<ApiResponseWrapper<AuthResponse>> RefreshToken(RefreshTokenCommand request)
    {
        return await httpService.Post<RefreshTokenCommand, AuthResponse>("account/RefreshToken", request);
    }

    #endregion Public Methods
}