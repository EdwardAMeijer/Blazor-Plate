namespace BinaryPlate.Application.Features.Account.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<Envelope<AuthResponse>>
{
    #region Public Properties

    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class RefreshTokenCommandHandler(ApplicationUserManager userManager,
                                            IUtcDateTimeProvider utcDateTimeProvider,
                                            ITokenGeneratorService tokenGeneratorService,
                                            IAppSettingsService appSettingsService) : IRequestHandler<RefreshTokenCommand, Envelope<AuthResponse>>
    {
        #region Public Methods

        public async Task<Envelope<AuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            // Check if request is null.
            if (request is null)
                return Envelope<AuthResponse>.Result.BadRequest(Resource.Invalid_client_request);

            // Get the principal from the expired access token.
            var principal = tokenGeneratorService.GetPrincipalFromExpiredToken(request.AccessToken);

            // Check if principal identity is null.
            if (principal.Identity is null)
                return Envelope<AuthResponse>.Result.BadRequest(Resource.Invalid_client_request);

            // Get the username from the principal.
            var username = principal.Identity.Name;

            // Find the user by email.
            var user = await userManager.FindByEmailAsync(username ?? throw new ArgumentNullException(nameof(username)));

            // Check if user is null or the refresh token is invalid.
            if (user is null || (user.RefreshToken != request.RefreshToken && user.RefreshTokenTimeSpan <= DateTime.UtcNow))
                return Envelope<AuthResponse>.Result.BadRequest(Resource.Invalid_client_request);

            // Get token settings from app settings use case.
            var tokenSettingsEnvelope = await appSettingsService.GetTokenSettings();

            // Get the refresh token time span from token settings.
            var refreshTokenTimeSpan = tokenSettingsEnvelope.Payload.RefreshTokenTimeSpan;

            // Check if refresh token time span is null.
            if (refreshTokenTimeSpan == null)
                return Envelope<AuthResponse>.Result.BadRequest(Resource.Refresh_token_timespan_cannot_be_null);

            // Update user refresh token time span.
            user.RefreshTokenTimeSpan = utcDateTimeProvider.GetUtcNow().AddMinutes(refreshTokenTimeSpan.Value);

            // Generate new access token for the user.
            var accessToken = await tokenGeneratorService.GenerateAccessTokenAsync(user);

            // Generate new refresh token for the user.
            var refreshToken = tokenGeneratorService.GenerateRefreshToken();

            // Update user refresh token.
            user.RefreshToken = refreshToken;

            // Update user with new refresh token and refresh token time span.
            var identityResult = await userManager.UpdateAsync(user);

            // Return error envelope if user update failed.
            if (!identityResult.Succeeded)
                return Envelope<AuthResponse>.Result.AddErrors(identityResult.Errors.ToApplicationResult(), HttpStatusCode.InternalServerError, rollbackDisabled: true);

            // Create and return new auth response.
            var response = new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = user.RefreshToken,
            };

            return Envelope<AuthResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}