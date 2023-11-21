using BinaryPlate.Application.Features.AppSettings.Commands.UpdateSettings.UpdateIdentitySettings.Models;

namespace BinaryPlate.Application.Features.AppSettings.Commands.UpdateSettings.UpdateIdentitySettings;

public class UpdateIdentitySettingsCommand : IRequest<Envelope<GetIdentitySettingsResponse>>
{
    #region Public Properties

    public UserSettingsModel UserSettingsModel { get; set; }
    public PasswordSettingsModel PasswordSettingsModel { get; set; }
    public LockoutSettingsModel LockoutSettingsModel { get; set; }
    public SignInSettingsModel SignInSettingsModel { get; set; }

    #endregion Public Properties

    #region Public Methods

    public void MapToEntity(UserSettings userSettings, PasswordSettings passwordSettings, LockoutSettings lockoutSettings, SignInSettings signInSettings)
    {
        userSettings.Id = Guid.Parse(UserSettingsModel.Id);
        userSettings.AllowedUserNameCharacters = UserSettingsModel.AllowedUserNameCharacters;
        userSettings.NewUsersActiveByDefault = UserSettingsModel.NewUsersActiveByDefault;
        userSettings.ConcurrencyStamp = UserSettingsModel.ConcurrencyStamp;

        passwordSettings.Id = Guid.Parse(PasswordSettingsModel.Id);
        passwordSettings.RequiredLength = PasswordSettingsModel.RequiredLength ?? throw new ArgumentNullException(nameof(PasswordSettingsModel.RequiredLength));
        passwordSettings.RequiredUniqueChars = PasswordSettingsModel.RequiredUniqueChars ?? throw new ArgumentNullException(nameof(PasswordSettingsModel.RequiredUniqueChars));
        passwordSettings.RequireNonAlphanumeric = PasswordSettingsModel.RequireNonAlphanumeric;
        passwordSettings.RequireLowercase = PasswordSettingsModel.RequireLowercase;
        passwordSettings.RequireUppercase = PasswordSettingsModel.RequireUppercase;
        passwordSettings.RequireDigit = PasswordSettingsModel.RequireDigit;
        passwordSettings.ConcurrencyStamp = PasswordSettingsModel.ConcurrencyStamp;

        lockoutSettings.Id = Guid.Parse(LockoutSettingsModel.Id);
        lockoutSettings.AllowedForNewUsers = LockoutSettingsModel.AllowedForNewUsers;
        lockoutSettings.MaxFailedAccessAttempts = LockoutSettingsModel.MaxFailedAccessAttempts ?? throw new ArgumentNullException(nameof(LockoutSettingsModel.MaxFailedAccessAttempts));
        lockoutSettings.DefaultLockoutTimeSpan = LockoutSettingsModel.DefaultLockoutTimeSpan ?? throw new ArgumentNullException(nameof(LockoutSettingsModel.DefaultLockoutTimeSpan));
        lockoutSettings.ConcurrencyStamp = LockoutSettingsModel.ConcurrencyStamp;

        signInSettings.Id = Guid.Parse(SignInSettingsModel.Id);
        //signInSettings.RequireConfirmedEmail =SignInSettings.RequireConfirmedEmail;
        //signInSettings.RequireConfirmedPhoneNumber =SignInSettings.RequireConfirmedPhoneNumber;
        signInSettings.RequireConfirmedAccount = SignInSettingsModel.RequireConfirmedAccount;
        signInSettings.ConcurrencyStamp = SignInSettingsModel.ConcurrencyStamp;
    }

    #endregion Public Methods

    #region Public Classes

    public class UpdateIdentitySettingsCommandHandler(IApplicationDbContext dbContext,
                                                      IAppOptionsService appOptionsService) : IRequestHandler<UpdateIdentitySettingsCommand, Envelope<GetIdentitySettingsResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetIdentitySettingsResponse>> Handle(UpdateIdentitySettingsCommand request, CancellationToken cancellationToken)
        {
            // Check whether the user settings ID is valid, and parse it into userSettingsId.
            if (!Guid.TryParse(request.UserSettingsModel.Id, out var userSettingsId))
                return Envelope<GetIdentitySettingsResponse>.Result.BadRequest(Resource.Invalid_user_settings_Id);

            // Check whether the password settings ID is valid, and parse it into passwordSettingsId.
            if (!Guid.TryParse(request.PasswordSettingsModel.Id, out var passwordSettingsId))
                return Envelope<GetIdentitySettingsResponse>.Result.BadRequest(Resource.Invalid_password_settings_Id);

            // Check whether the lockout settings ID is valid, and parse it into lockoutSettingsId.
            if (!Guid.TryParse(request.LockoutSettingsModel.Id, out var lockoutSettingsId))
                return Envelope<GetIdentitySettingsResponse>.Result.BadRequest(Resource.Invalid_lockout_settings_Id);

            // Check whether the sign in settings ID is valid, and parse it into signInSettingsId.
            if (!Guid.TryParse(request.SignInSettingsModel.Id, out var signInSettingsId))
                return Envelope<GetIdentitySettingsResponse>.Result.BadRequest(Resource.Invalid_sign_in_settings_Id);

            // Retrieve the user settings that match userSettingsId from the database, or get the
            // default AppUserOptions.
            var userSettings = await dbContext.UserSettings.FirstOrDefaultAsync(us => us.Id == userSettingsId, cancellationToken: cancellationToken)
                               ?? appOptionsService.GetAppUserOptions().MapToEntity();

            // Retrieve the password settings that match passwordSettingsId from the database, or
            // get the default AppPasswordOptions.
            var passwordSettings = await dbContext.PasswordSettings.FirstOrDefaultAsync(ps => ps.Id == passwordSettingsId, cancellationToken: cancellationToken)
                                   ?? appOptionsService.GetAppPasswordOptions().MapToEntity();

            // Retrieve the lockout settings that match lockoutSettingsId from the database, or get
            // the default AppLockoutOptions.
            var lockoutSettings = await dbContext.LockoutSettings.FirstOrDefaultAsync(ls => ls.Id == lockoutSettingsId, cancellationToken: cancellationToken)
                                  ?? appOptionsService.GetAppLockoutOptions().MapToEntity();

            // Retrieve the sign in settings that match signInSettingsId from the database, or get
            // the default AppSignInOptions.
            var signInSettings = await dbContext.SignInSettings.FirstOrDefaultAsync(ss => ss.Id == signInSettingsId, cancellationToken: cancellationToken)
                                 ?? appOptionsService.GetAppSignInOptions().MapToEntity();

            // Map properties from the request to the retrieved or default settings entities
            request.MapToEntity(userSettings, passwordSettings, lockoutSettings, signInSettings);

            // Update the UserSettings table with userSettings.
            dbContext.UserSettings.Update(userSettings);

            // Update the PasswordSettings table with passwordSettings.
            dbContext.PasswordSettings.Update(passwordSettings);

            // Update the LockoutSettings table with lockoutSettings.
            dbContext.LockoutSettings.Update(lockoutSettings);

            // Update the SignInSettings table with signInSettings.
            dbContext.SignInSettings.Update(signInSettings);

            // Save changes to the database.
            await dbContext.SaveChangesAsync(cancellationToken);

            // Create a new GetIdentitySettingsResponse object with the updated settings and success message.
            var response = new GetIdentitySettingsResponse
            {
                LockoutSettingsId = lockoutSettings.Id,
                PasswordSettingsId = passwordSettings.Id,
                SignInSettingsId = signInSettings.Id,
                UserSettingsId = userSettings.Id,
                LockoutSettingsConcurrencyStamp = lockoutSettings.ConcurrencyStamp,
                PasswordSettingsConcurrencyStamp = passwordSettings.ConcurrencyStamp,
                SignInSettingsConcurrencyStamp = signInSettings.ConcurrencyStamp,
                UserSettingsConcurrencyStamp = userSettings.ConcurrencyStamp,
                SuccessMessage = Resource.Identity_settings_have_been_updated_successfully
            };

            // Return an envelope with the updated GetIdentitySettingsResponse object.
            return Envelope<GetIdentitySettingsResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}