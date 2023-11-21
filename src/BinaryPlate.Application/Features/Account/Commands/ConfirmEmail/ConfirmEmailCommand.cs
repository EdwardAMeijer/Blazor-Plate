namespace BinaryPlate.Application.Features.Account.Commands.ConfirmEmail;

public class ConfirmEmailCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public string UserId { get; set; }
    public string Code { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class ConfirmEmailCommandHandler(ApplicationUserManager userManager) : IRequestHandler<ConfirmEmailCommand, Envelope<string>>
    {
        #region Public Methods

        public async Task<Envelope<string>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            // Check if the user ID is valid.
            if (string.IsNullOrWhiteSpace(request.UserId))
                return Envelope<string>.Result.BadRequest(Resource.Invalid_user_Id);

            // Find the user by their ID.
            var user = await userManager.FindByIdAsync(request.UserId);

            // If user is not found, return success message to avoid revealing that the user does
            // not exist.
            if (user == null)
                return Envelope<string>.Result.Ok(Resource.User_email_has_been_confirmed_successfully);

            // Decode the confirmation code.
            request.Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));

            // Confirm the user's email address.
            var identityResult = await userManager.ConfirmEmailAsync(user, request.Code);

            // If confirmation fails, return errors.
            return !identityResult.Succeeded
                ? Envelope<string>.Result.AddErrors(identityResult.Errors.ToApplicationResult(), HttpStatusCode.InternalServerError, rollbackDisabled: true)
                : Envelope<string>.Result.Ok(Resource.User_email_has_been_confirmed_successfully);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}