namespace BinaryPlate.Application.Services;

public class AppSettingsService(IApplicationDbContext dbContext, IAppOptionsService appOptionsService) : IAppSettingsService
{
    #region Public Methods

    public async Task<Envelope<GetIdentitySettingsForEditResponse>> GetIdentitySettings()
    {
        // Retrieve UserSettings entity from the database, or fall back to the default settings if
        // not found.
        var userSettings = await dbContext.UserSettings.FirstOrDefaultAsync()
                           ?? appOptionsService.GetAppUserOptions().MapToEntity();

        // Retrieve PasswordSettings entity from the database, or if not found, create a new one
        // from the configuration options.
        var passwordSettings = await dbContext.PasswordSettings.FirstOrDefaultAsync()
                               ?? appOptionsService.GetAppPasswordOptions().MapToEntity();

        // Retrieve LockoutSettings entity from the database, or fall back to the default settings
        // if not found.
        var lockoutSettings = await dbContext.LockoutSettings.FirstOrDefaultAsync()
                              ?? appOptionsService.GetAppLockoutOptions().MapToEntity();

        // Retrieve SignInSettings entity from the database, or fall back to the default settings if
        // not found.
        var signInSettings = await dbContext.SignInSettings.FirstOrDefaultAsync()
                             ?? appOptionsService.GetAppSignInOptions().MapToEntity();

        // Map the retrieved entities to DTOs.
        var response = new GetIdentitySettingsForEditResponse
        {
            UserSettingsForEdit = UserSettingsForEdit.MapFromEntity(userSettings),
            PasswordSettingsForEdit = PasswordSettingsForEdit.MapFromEntity(passwordSettings),
            LockoutSettingsForEdit = LockoutSettingsForEdit.MapFromEntity(lockoutSettings),
            SignInSettingsForEdit = SignInSettingsForEdit.MapFromEntity(signInSettings)
        };

        // Return the DTO wrapped in an Envelope object.
        return Envelope<GetIdentitySettingsForEditResponse>.Result.Ok(response);
    }

    public async Task<Envelope<GetTokenSettingsForEditResponse>> GetTokenSettings()
    {
        // Retrieve the current token settings from the database, or fall back to the default
        // settings if not found.
        var tokenSettings = await dbContext.TokenSettings.FirstOrDefaultAsync()
                            ?? appOptionsService.GetAppTokenOptions().MapToEntity();

        // Map the token settings entity onto a response DTO.
        var tokenSettingsForEditResponse = GetTokenSettingsForEditResponse.MapFromEntity(tokenSettings);

        // Return the response envelope containing the token settings DTO.
        return Envelope<GetTokenSettingsForEditResponse>.Result.Ok(tokenSettingsForEditResponse);
    }

    public async Task<Envelope<GetFileStorageSettingsForEditResponse>> GetFileStorageSettings()
    {
        // Get the file storage settings entity from the database, or fall back to the default
        // settings if not found.
        var fileStorageSettings = await dbContext.FileStorageSettings.FirstOrDefaultAsync()
                                  ?? appOptionsService.GetAppFileStorageOptions().MapToEntity();

        // Map the entity to a response DTO.
        var storageSettingsForEditResponse = GetFileStorageSettingsForEditResponse.MapFromEntity(fileStorageSettings);

        // Return the response in an envelope with a success status.
        return Envelope<GetFileStorageSettingsForEditResponse>.Result.Ok(storageSettingsForEditResponse);
    }

    #endregion Public Methods
}