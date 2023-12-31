﻿namespace BinaryPlate.Application.Features.Account.Commands.Register;

public class RegisterCommand : IRequest<Envelope<RegisterResponse>>
{
    #region Public Properties

    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string ReturnUrl { get; set; }

    #endregion Public Properties

    #region Public Methods

    public ApplicationUser MapToEntity()
    {
        return new()
        {
            UserName = Email,
            Email = Email,
        };
    }

    #endregion Public Methods

    #region Public Classes

    public class RegisterCommandHandler(ApplicationUserManager userManager,
                                        ApplicationRoleManager roleManager,
                                        IApplicationDbContext dbContext,
                                        IAuthService authService,
                                        IAppSettingsService appSettingsService) : IRequestHandler<RegisterCommand, Envelope<RegisterResponse>>
    {
        #region Public Methods

        public async Task<Envelope<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Create new user entity from the request data.
            var user = request.MapToEntity();

            // Assign default roles to the new user.
            AssignDefaultRolesToUser(user);

            // Set the initial activation status for the new user.
            await SetInitialActivation(user);

            // Attempt to create the new user with the provided password.
            var createUserResult = await userManager.CreateAsync(user, request.Password);

            // If user creation is not successful, return a server error response.
            if (!createUserResult.Succeeded)
                return Envelope<RegisterResponse>.Result.AddErrors(createUserResult.Errors.ToApplicationResult(),
                                                                   HttpStatusCode.InternalServerError);

            // Attempt to register the new user as a super admin if they are not already registered
            // as one.
            var registerAsSuperAdminEnvelope = await RegisterAsSuperAdminIfNotExist(user);

            // If registration as super admin is not successful, return a server error response.
            if (registerAsSuperAdminEnvelope.IsError)
                return Envelope<RegisterResponse>.Result.AddErrors(createUserResult.Errors.ToApplicationResult(),
                                                                   HttpStatusCode.InternalServerError);

            // Check if email confirmation is required for registration.
            switch (userManager.Options.SignIn.RequireConfirmedAccount)
            {
                case true:
                    {
                        // If email confirmation is required, send activation email to the user.
                        var callbackUrl = await userManager.SendActivationEmailAsync(user);

                        // Create a response with the confirmation URL.
                        var response = new RegisterResponse
                        {
                            RequireConfirmedAccount = true,
                            DisplayConfirmAccountLink = true,
                            Email = user.Email,
                            EmailConfirmationUrl = HttpUtility.UrlEncode(callbackUrl),
                            AuthResponse = null,
                            SuccessMessage = Resource.Verification_email_has_been_sent
                        };

                        return Envelope<RegisterResponse>.Result.Ok(response);
                    }
                default:
                    {
                        // If email confirmation is not required, log in the user and return a
                        // response with auth tokens.
                        var loginCommand = new LoginCommand
                        {
                            Email = request.Email,
                            Password = request.Password,
                        };

                        // call the Login method passing in the loginCommand and await for the response.
                        var loginResponse = await authService.Login(loginCommand);

                        // if the response from Login method has an error.
                        if (loginResponse.IsError)
                            return loginResponse.ValidationErrors.Any()
                                ? Envelope<RegisterResponse>.Result.AddErrors(loginResponse.ValidationErrors,
                                                                              HttpStatusCode.InternalServerError,
                                                                              rollbackDisabled: true)
                                : Envelope<RegisterResponse>.Result.ServerError(loginResponse.Title, rollbackDisabled: true); // return an error message based on the presence of ModelStateErrors in the loginResponse.

                        var response = new RegisterResponse
                        // create a new instance of RegisterResponse and set its properties.
                        {
                            RequireConfirmedAccount = false,
                            DisplayConfirmAccountLink = false,
                            Email = user.Email,
                            EmailConfirmationUrl = null,
                            AuthResponse = new AuthResponse
                            {
                                AccessToken = loginResponse.Payload.AuthResponse.AccessToken,
                                RefreshToken = loginResponse.Payload.AuthResponse.RefreshToken
                            },
                            SuccessMessage = Resource.You_have_successfully_created_a_new_account
                        };

                        // return an OK response with the newly created RegisterResponse instance.
                        return Envelope<RegisterResponse>.Result.Ok(response);
                    }
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void AssignDefaultRolesToUser(ApplicationUser user)
        {
            // Get the IDs of all roles that are marked as default.
            var defaultRoleIds = roleManager.Roles.Where(r => r.IsDefault).Select(r => r.Id);

            // For each default role ID, create a new ApplicationUserRole object and add it to the
            // user's UserRoles collection.
            foreach (var defaultRoleId in defaultRoleIds)
                user.UserRoles.Add(new ApplicationUserRole { RoleId = defaultRoleId });
        }

        private async Task SetInitialActivation(ApplicationUser user)
        {
            // Get the identity settings from the app settings use case.
            var identitySettings = await appSettingsService.GetIdentitySettings();

            // Set the user's IsSuspended property based on whether new users are active by default
            // or not.
            user.IsSuspended = !identitySettings.Payload.UserSettingsForEdit.NewUsersActiveByDefault;
        }

        private async Task<Envelope<ApplicationUser>> RegisterAsSuperAdminIfNotExist(ApplicationUser user)
        {
            // Check if a super admin already exists in the system.
            var isSuperAdminExist = await userManager.Users.CountAsync(u => u.IsSuperAdmin) > 0;

            // If a super admin already exists, return success envelope.
            if (isSuperAdminExist)
                return Envelope<ApplicationUser>.Result.Ok();

            // Create a new admin role.
            var adminRole = new ApplicationRole
            {
                Name = "Admin",
                IsStatic = true
            };

            // Check if the admin role exists in the system.
            var isAdminRoleExists = await roleManager.RoleExistsAsync(adminRole.Name);

            // If the admin role does not exist, create it.
            if (!isAdminRoleExists)
            {
                // Grant permissions for the admin role.
                await GrantPermissionsForAdminRole(adminRole);

                // Create the admin role.
                var roleResult = await roleManager.CreateAsync(adminRole);

                // If the creation of the admin role failed, return an error envelope.
                if (!roleResult.Succeeded)
                    return Envelope<ApplicationUser>.Result.AddErrors(roleResult.Errors.ToApplicationResult(),
                                                                      HttpStatusCode.BadRequest);
            }

            // Get the number of users in the admin role.
            var adminUserCount = await userManager.GetUsersInRoleCountAsync(adminRole.Name);

            // If there are users in the admin role, return success envelope.
            if (adminUserCount > 0)
                return Envelope<ApplicationUser>.Result.Ok();

            // Set the user properties to make them a super admin.
            user.IsStatic = true;
            user.IsSuperAdmin = true;
            user.JobTitle = "Administrator";

            // Add the user to the admin role.
            var identityResult = await userManager.AddToRoleAsync(user, adminRole.Name);

            // If adding the user to the admin role failed, return an error envelope.
            if (!identityResult.Succeeded)
                return Envelope<ApplicationUser>.Result.AddErrors(identityResult.Errors.ToApplicationResult(),
                                                                  HttpStatusCode.BadRequest);

            // Return success envelope with the user object.
            return Envelope<ApplicationUser>.Result.Ok(user);
        }

        private async Task GrantPermissionsForAdminRole(ApplicationRole adminRole)
        {
            // get admin permissions.
            var adminPermissions = await dbContext.ApplicationPermissions.ToListAsync();

            // add a role claim for each permission to the adminRole.
            foreach (var permission in adminPermissions)
                adminRole.RoleClaims.Add(new ApplicationRoleClaim
                {
                    ClaimType = "permissions",
                    ClaimValue = permission.Name
                });
        }

        #endregion Private Methods
    }

    #endregion Public Classes
}