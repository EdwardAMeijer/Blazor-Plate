namespace BinaryPlate.Application.Features.Account.Commands.ResendEmailConfirmation;

public class ResendEmailConfirmationCommand : IRequest<Envelope<ResendEmailConfirmationResponse>>
{
    #region Public Properties

    public string Email { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class ResendEmailConfirmationCommandHandler(ApplicationUserManager userManager) : IRequestHandler<ResendEmailConfirmationCommand, Envelope<ResendEmailConfirmationResponse>>
    {
        #region Public Methods

        public async Task<Envelope<ResendEmailConfirmationResponse>> Handle(ResendEmailConfirmationCommand request, CancellationToken cancellationToken)
        {
            ResendEmailConfirmationResponse response;

            // Find the user by email address.
            var user = await userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                // Don't reveal that the user does not exist.
                response = new ResendEmailConfirmationResponse
                {
                    RequireConfirmedAccount = true,
                    DisplayConfirmAccountLink = true,
                    SuccessMessage = Resource.Verification_email_has_been_sent
                };
                return Envelope<ResendEmailConfirmationResponse>.Result.Ok(response);
            }

            // Send the activation email and get the callback URL.
            var callbackUrl = await userManager.SendActivationEmailAsync(user);

            // Return a successful response with the confirmation URL.
            response = new ResendEmailConfirmationResponse
            {
                RequireConfirmedAccount = true,
                DisplayConfirmAccountLink = true,
                EmailConfirmationUrl = HttpUtility.UrlEncode(callbackUrl),
                SuccessMessage = Resource.Verification_email_has_been_sent
            };

            return Envelope<ResendEmailConfirmationResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}