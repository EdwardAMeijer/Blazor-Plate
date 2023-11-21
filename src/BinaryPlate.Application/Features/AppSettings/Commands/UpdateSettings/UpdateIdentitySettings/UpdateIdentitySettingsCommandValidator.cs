namespace BinaryPlate.Application.Features.AppSettings.Commands.UpdateSettings.UpdateIdentitySettings;

public class UpdateIdentitySettingsCommandValidator : AbstractValidator<UpdateIdentitySettingsCommand>
{
    #region Public Constructors

    public UpdateIdentitySettingsCommandValidator()
    {
        RuleFor(v => v.UserSettingsModel.AllowedUserNameCharacters).Cascade(CascadeMode.Stop)
                                                                   .NotEmpty()
                                                                   .WithMessage(Resource.Allowed_username_characters_are_required);

        RuleFor(v => v.LockoutSettingsModel.DefaultLockoutTimeSpan).Cascade(CascadeMode.Stop)
                                                                   .NotEmpty()
                                                                   .WithMessage(Resource.Default_lockout_time_Span_is_required);

        RuleFor(v => v.LockoutSettingsModel.MaxFailedAccessAttempts).Cascade(CascadeMode.Stop)
                                                                    .NotEmpty()
                                                                    .WithMessage(Resource.Max_failed_access_attempt_is_required);

        RuleFor(v => v.PasswordSettingsModel.RequiredLength).Cascade(CascadeMode.Stop)
                                                              .NotEmpty()
                                                              .WithMessage(Resource.Required_length_is_required);

        RuleFor(v => v.PasswordSettingsModel.RequiredUniqueChars).Cascade(CascadeMode.Stop)
                                                                   .NotEmpty()
                                                                   .WithMessage(Resource.Required_unique_characters_is_required);
    }

    #endregion Public Constructors
}