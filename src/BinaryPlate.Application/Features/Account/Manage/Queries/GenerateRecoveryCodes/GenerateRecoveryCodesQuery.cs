namespace BinaryPlate.Application.Features.Account.Manage.Queries.GenerateRecoveryCodes;

public class GenerateRecoveryCodesQuery : IRequest<Envelope<GenerateRecoveryCodesResponse>>
{
    #region Public Classes

    public class GenerateRecoveryCodesQueryHandler(ApplicationUserManager userManager, 
                                                   IHttpContextAccessor httpContextAccessor) : IRequestHandler<GenerateRecoveryCodesQuery, Envelope<GenerateRecoveryCodesResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GenerateRecoveryCodesResponse>> Handle(GenerateRecoveryCodesQuery request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = httpContextAccessor.GetUserId();

            // Check if the user ID is null or empty.
            if (string.IsNullOrEmpty(userId))
                return Envelope<GenerateRecoveryCodesResponse>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user based on the user ID.
            var user = await userManager.FindByIdAsync(userId);

            // If the user is not found, return an unauthorized error.
            if (user == null)
                return Envelope<GenerateRecoveryCodesResponse>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Check if the user has 2FA enabled.
            var isTwoFactorEnabled = await userManager.GetTwoFactorEnabledAsync(user);

            // If 2FA is not enabled, return an error.
            if (!isTwoFactorEnabled)
                return Envelope<GenerateRecoveryCodesResponse>.Result.ServerError(string.Format(Resource.Cannot_generate_recovery_codes, user.UserName));

            // Generate new recovery codes for the user.
            var response = new GenerateRecoveryCodesResponse
            {
                RecoveryCodes = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10)
            };

            // Convert the recovery codes to an array.
            response.RecoveryCodes = response.RecoveryCodes?.ToArray();

            // Set the success message for the response.
            response.StatusMessage = Resource.You_have_generated_new_recovery_codes;

            // Return the response wrapped in an Envelope.
            return Envelope<GenerateRecoveryCodesResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}