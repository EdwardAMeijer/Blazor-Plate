namespace BinaryPlate.Infrastructure.Services;

public class AppOptionsService(IOptionsSnapshot<AppOptions> appOptionsSnapshot) : IAppOptionsService
{
    #region Private Fields

    private readonly AppOptions _appOptionsSnapshot = appOptionsSnapshot.Value;

    #endregion Private Fields

    #region Public Methods

    public AppClientOptions GetAppClientOptions()
    {
        return _appOptionsSnapshot.AppClientOptions;
    }

    public AppJwtOptions GetAppJwtOptions()
    {
        return _appOptionsSnapshot.AppJwtOptions;
    }

    public AppMailSenderOptions GetAppMailSenderOptions()
    {
        return _appOptionsSnapshot.AppMailSenderOptions;
    }

    public AppUserOptions GetAppUserOptions()
    {
        return _appOptionsSnapshot.AppIdentityOptions.AppUserOptions;
    }

    public AppPasswordOptions GetAppPasswordOptions()
    {
        return _appOptionsSnapshot.AppIdentityOptions.AppPasswordOptions;
    }

    public AppLockoutOptions GetAppLockoutOptions()
    {
        return _appOptionsSnapshot.AppIdentityOptions.AppLockoutOptions;
    }

    public AppSignInOptions GetAppSignInOptions()
    {
        return _appOptionsSnapshot.AppIdentityOptions.AppSignInOptions;
    }

    public AppTokenOptions GetAppTokenOptions()
    {
        return _appOptionsSnapshot.AppTokenOptions;
    }

    public AppFileStorageOptions GetAppFileStorageOptions()
    {
        return _appOptionsSnapshot.AppFileStorageOptions;
    }

    #endregion Public Methods
}