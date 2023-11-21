namespace BinaryPlate.Application.Features.Identity.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public string Id { get; set; }

    #endregion Public Properties

    public class DeleteUserCommandHandler(ApplicationUserManager userManager) : IRequestHandler<DeleteUserCommand, Envelope<string>>
    {
        #region Public Methods

        #region Public Methods

        public async Task<Envelope<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            // Find the user by their ID using the user manager.
            var user = await userManager.FindByIdAsync(request.Id);

            // If the user is not found, return a NotFound response with an error message.
            if (user == null)
                return Envelope<string>.Result.NotFound(Resource.Unable_to_load_user);

            // If the user is a static user, return a ServerError response with an error message.
            if (user.IsStatic)
                return Envelope<string>.Result.ServerError(Resource.Unable_to_delete_static_user);

            // Delete the user using the user manager and return an OK response if successful, or a
            // ServerError response with the errors if not.
            var identityResult = await userManager.DeleteAsync(user);

            return !identityResult.Succeeded
                ? Envelope<string>.Result.AddErrors(identityResult.Errors.ToApplicationResult(), HttpStatusCode.InternalServerError)
                : Envelope<string>.Result.Ok(Resource.User_has_been_deleted_successfully);
        }

        #endregion Public Methods
    }

    #endregion Public Methods
}