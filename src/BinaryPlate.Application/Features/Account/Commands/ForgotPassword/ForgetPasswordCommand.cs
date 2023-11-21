namespace BinaryPlate.Application.Features.Account.Commands.ForgotPassword;

public class ForgetPasswordCommand : IRequest<Envelope<ForgetPasswordResponse>>
{
    #region Public Properties

    public string Email { get; set; }

    #endregion Public Properties

    #region Public Classes

    public class ForgotPasswordCommandHandler(ApplicationUserManager userManager) : IRequestHandler<ForgetPasswordCommand, Envelope<ForgetPasswordResponse>>
    {
        #region Public Methods

        public async Task<Envelope<ForgetPasswordResponse>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            // Find the user associated with the email address provided by the user.
            var user = await userManager.FindByEmailAsync(request.Email);

            // If no user is not found return a success message without revealing the reason for failure.
            if (user is null)
                return Envelope<ForgetPasswordResponse>.Result.Ok(new ForgetPasswordResponse
                {
                    Code = null,
                    SuccessMessage = Resource.Password_reset_link_was_sent
                });

            // Check if the user has a password set.
            var userHasPassword = await userManager.HasPasswordAsync(user);

            // If no user is found or the user's email is not confirmed or they don't have a
            // password, return a success message without revealing the reason for failure.
            if (user is not { EmailConfirmed: true } || !userHasPassword)
                return Envelope<ForgetPasswordResponse>.Result.Ok(new ForgetPasswordResponse
                {
                    Code = null,
                    SuccessMessage = Resource.Password_reset_link_was_sent
                });

            // If the user exists, send a password reset code to their email address.
            var code = await userManager.SendResetPasswordAsync(user);

            // Create a response object that includes the password reset code and a success message.
            var response = new ForgetPasswordResponse
            {
                Code = code,
                DisplayConfirmPasswordLink = true,
                SuccessMessage = Resource.Password_reset_link_was_sent,
            };

            // Return the response object in an envelope with a success status.
            return Envelope<ForgetPasswordResponse>.Result.Ok(response);
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}