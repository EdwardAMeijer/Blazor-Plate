namespace BinaryPlate.Application.Features.Account.Manage.Queries.LoadSharedKeyAndQrCodeUri;

public class LoadSharedKeyAndQrCodeUriQuery : IRequest<Envelope<LoadSharedKeyAndQrCodeUriResponse>>
{
    #region Public Classes

    public class LoadSharedKeyAndQrCodeUriQueryHandler(ApplicationUserManager userManager,
                                                       UrlEncoder urlEncoder,
                                                       IHttpContextAccessor httpContextAccessor) : IRequestHandler<LoadSharedKeyAndQrCodeUriQuery, Envelope<LoadSharedKeyAndQrCodeUriResponse>>
    {
        #region Private Fields

        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        #endregion Private Fields

        #region Public Methods

        public async Task<Envelope<LoadSharedKeyAndQrCodeUriResponse>> Handle(LoadSharedKeyAndQrCodeUriQuery request, CancellationToken cancellationToken)
        {
            // Get the current user ID from the HttpContext.
            var userId = httpContextAccessor.GetUserId();

            // Check if the user id is null or empty.
            if (string.IsNullOrEmpty(userId))
                return Envelope<LoadSharedKeyAndQrCodeUriResponse>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user based on the user id.
            var user = await userManager.FindByIdAsync(userId);

            // Check if the user is null.
            if (user == null)
                return Envelope<LoadSharedKeyAndQrCodeUriResponse>.Result.Unauthorized(Resource.Unable_to_load_user);

            // Load the authenticator key & QR code URI to display on the form.
            var unformattedKey = await userManager.GetAuthenticatorKeyAsync(user);

            // Check if the authenticator key is null or empty.
            if (string.IsNullOrEmpty(unformattedKey))
            {
                // Reset the authenticator key if it is null or empty.
                await userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await userManager.GetAuthenticatorKeyAsync(user);
            }

            // Format the authenticator key.
            var response = new LoadSharedKeyAndQrCodeUriResponse
            {
                SharedKey = FormatKey(unformattedKey)
            };

            // Get the user email.
            var email = await userManager.GetEmailAsync(user);

            // Generate the QR code URI based on the user email and authenticator key.
            response.AuthenticatorUri = GenerateQrCodeUri(email, unformattedKey);

            // Return the result.
            return Envelope<LoadSharedKeyAndQrCodeUriResponse>.Result.Ok(response);
        }

        #endregion Public Methods

        #region Private Methods

        private static string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            var currentPosition = 0;

            // Split the unformattedKey into groups of four characters separated by spaces.
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }

            // Append any remaining characters that are less than four.
            if (currentPosition < unformattedKey.Length)
                result.Append(unformattedKey.Substring(currentPosition));

            // Convert the result to lowercase and return it.
            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            // Format the authenticator URI using the provided parameters and URL-encode them.
            return string.Format(AuthenticatorUriFormat,
                                 urlEncoder.Encode("BinaryPlate"),
                                 urlEncoder.Encode(email),
                                 unformattedKey);
        }

        #endregion Private Methods
    }

    #endregion Public Classes
}