namespace BinaryPlate.Application.Features.Identity.Users.Commands.GrantOrRevokeUserPermissions;

public class GrantOrRevokeUserPermissionsCommand : IRequest<Envelope<string>>
{
    #region Public Properties

    public string UserId { get; set; }
    public IReadOnlyList<Guid> SelectedPermissionIds { get; set; }

    #endregion Public Properties

    public class GrantOrRevokeUserPermissionsCommandHandler(ApplicationUserManager userManager) : IRequestHandler<GrantOrRevokeUserPermissionsCommand, Envelope<string>>
    {
        #region Public Methods

        #region Public Methods

        public async Task<Envelope<string>> Handle(GrantOrRevokeUserPermissionsCommand request, CancellationToken cancellationToken)
        {
            // Get the user with the given ID, including their claims and roles.
            var user = await userManager.Users.Include(u => u.Claims)
                                         .Include(u => u.UserRoles)
                                         .Where(u => u.Id == request.UserId)
                                         .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            // Return a "not found" result if the user doesn't exist.
            if (user == null)
                return Envelope<string>.Result.NotFound(Resource.Unable_to_load_user);

            // Return a "not found" result if the user is static and can't be updated.
            if (user.IsStatic)
                return Envelope<string>.Result.NotFound(Resource.Unable_to_update_static_user);

            // Add or remove the selected permissions for the user.
            await userManager.AddOrRemovePermissionsForUserAsync(request.SelectedPermissionIds, user);

            // Update the user with the changes.
            var identityResult = await userManager.UpdateAsync(user);

            // Return a successful result if the identity update succeeded, otherwise return an
            // error result with the error messages.
            return !identityResult.Succeeded
                ? Envelope<string>.Result.AddErrors(identityResult.Errors.ToApplicationResult(), HttpStatusCode.InternalServerError)
                : Envelope<string>.Result.Ok(Resource.User_permissions_have_been_updated_successfully);
        }

        #endregion Public Methods
    }

    #endregion Public Methods
}