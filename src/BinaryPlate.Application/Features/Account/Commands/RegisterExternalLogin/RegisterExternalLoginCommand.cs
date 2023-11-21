namespace BinaryPlate.Application.Features.Account.Commands.RegisterExternalLogin;

public class RegisterExternalLoginCommand(UserInfo userInfo) : IRequest<Envelope<RegisterExternalLoginResponse>>
{
    #region Public Properties

    public UserInfo UserInfo { get; } = userInfo;

    #endregion Public Properties

    #region Public Classes

    public class RegisterExternalLoginCommandCommandHandler(SignInManager<ApplicationUser> signInManager,
                                                            ApplicationUserManager userManager,
                                                            ApplicationRoleManager roleManager,
                                                            IApplicationDbContext dbContext,
                                                            IAuthService authService) : IRequestHandler<RegisterExternalLoginCommand, Envelope<RegisterExternalLoginResponse>>
    {
        #region Private Fields

        private const string ExternalLoginRole = "External-Login";

        #endregion Private Fields

        #region Public Methods

        public async Task<Envelope<RegisterExternalLoginResponse>> Handle(RegisterExternalLoginCommand request, CancellationToken cancellationToken)
        {
            // Find the user by email.
            var user = await userManager.FindByEmailAsync(request.UserInfo.Email);

            // If the user does not exist, create a new user.
            if (user == null)
            {
                // Instantiate the user object.
                user = new ApplicationUser
                {
                    UserName = request.UserInfo.Email, // Set the user's username as their email
                    Email = request.UserInfo.Email, // Set the user's email
                    Name = request.UserInfo.FirstName, // Set the user's first name
                    Surname = request.UserInfo.LastName, // Set the user's last name
                    EmailConfirmed = true, // Set the email confirmation status as true
                    IsSuperAdmin = true, // Remove this line before going to production.
                };

                // Create a new user with the provided information.
                var createUserResult = await userManager.CreateAsync(user);

                // Check if user creation succeeded.
                if (!createUserResult.Succeeded)
                    return Envelope<RegisterExternalLoginResponse>.Result.AddErrors(createUserResult.Errors.ToApplicationResult(), HttpStatusCode.InternalServerError);

                // Instantiate the external login role object.
                var externalLoginRole = new ApplicationRole
                {
                    Name = ExternalLoginRole,
                    IsStatic = true
                };

                // Check if the external login role exists, if not, create it.
                if (!await roleManager.RoleExistsAsync(externalLoginRole.Name))
                {
                    // Create the external login role.
                    var externalLoginRoleResult = await CreateRole(externalLoginRole);

                    // Check if external login role creation succeeded.
                    if (!externalLoginRoleResult.Succeeded)
                        return Envelope<RegisterExternalLoginResponse>.Result.AddErrors(externalLoginRoleResult.Errors.ToApplicationResult(), HttpStatusCode.InternalServerError);
                }

                // Add the user to the external login role.
                var addToRoleResult = await userManager.AddToRoleAsync(user, ExternalLoginRole);

                // Check if adding user to external login role succeeded.
                if (!addToRoleResult.Succeeded)
                    return Envelope<RegisterExternalLoginResponse>.Result.AddErrors(addToRoleResult.Errors.ToApplicationResult(), HttpStatusCode.InternalServerError);
            }

            // Create the principal for the external login.
            var principal = CreatePrincipal(request);

            // Check if the external login already exists for the user.
            var existingLogin = await userManager.FindByLoginAsync(request.UserInfo.LoginProvider, request.UserInfo.ProviderKey);

            // If the external login does not exist for the user, add it.
            if (existingLogin == null)
            {
                // Add the external login to the user.
                var addLoginResult = await CreateLogin(request, user, principal);

                // Check if adding external login to the user succeeded.
                if (!addLoginResult.Succeeded)
                    return Envelope<RegisterExternalLoginResponse>.Result.AddErrors(addLoginResult.Errors.ToApplicationResult(), HttpStatusCode.InternalServerError);
            }

            // Sign in the user using the external login.
            var signInResult = await signInManager.ExternalLoginSignInAsync(request.UserInfo.LoginProvider,
                                                                             request.UserInfo.ProviderKey,
                                                                             isPersistent: false,
                                                                             bypassTwoFactor: true);

            // Check if external login sign-in succeeded.
            if (signInResult.Succeeded)
            {
                // Check if the user has a password associated with their account.
                var hasPassword = (await userManager.HasPasswordAsync(user));

                // Generate access and refresh tokens for the user.
                var (accessToken, refreshToken) = await authService.GenerateAccessAndRefreshTokens(user, hasPassword);

                var response = new RegisterExternalLoginResponse
                {
                    AuthResponse = new AuthResponse
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    },
                    RequiresTwoFactor = false,
                };

                // Return the response with the claims.
                return Envelope<RegisterExternalLoginResponse>.Result.Ok(response);
            }

            // Return the error response if external login sign-in failed.
            return Envelope<RegisterExternalLoginResponse>.Result.AddErrors(signInResult.ToApplicationResult(), HttpStatusCode.InternalServerError);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Creates a ClaimsPrincipal object based on the provided RegisterExternalLoginCommand request.
        /// </summary>
        /// <param name="request">The RegisterExternalLoginCommand request.</param>
        /// <returns>The created ClaimsPrincipal object.</returns>
        private static ClaimsPrincipal CreatePrincipal(RegisterExternalLoginCommand request)
        {
            // Create a list to store the claims
            var claims = new List<Claim>();

            // Add the necessary claims to the list
            claims.AddRange(new[]
            {
                new Claim(ClaimTypes.Email, request.UserInfo.Email),
                new Claim(ClaimTypes.GivenName, request.UserInfo.FirstName),
                new Claim(ClaimTypes.Surname, request.UserInfo.LastName),
                new Claim(ClaimTypes.Name, request.UserInfo.DisplayName),
                new Claim(ClaimTypes.AuthenticationMethod, request.UserInfo.LoginProvider),
                new Claim(ClaimTypes.NameIdentifier, request.UserInfo.ProviderKey)
            });

            // Create an identity and assign it the claims
            var identity = new ClaimsIdentity(claims);

            // Create the principal using the identity
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }

        /// <summary>
        /// Creates an external login for the user based on the provided
        /// RegisterExternalLoginCommand request.
        /// </summary>
        /// <param name="request">The RegisterExternalLoginCommand request.</param>
        /// <param name="user">The ApplicationUser object representing the user.</param>
        /// <param name="principal">The ClaimsPrincipal object.</param>
        /// <returns>
        /// An IdentityResult indicating the result of adding the external login to the user.
        /// </returns>
        private async Task<IdentityResult> CreateLogin(RegisterExternalLoginCommand request, ApplicationUser user, ClaimsPrincipal principal)
        {
            // Instantiate the external login object.
            var externalLoginInfo = new ExternalLoginInfo(principal,
                                                          request.UserInfo.LoginProvider,
                                                          request.UserInfo.ProviderKey,
                                                          request.UserInfo.DisplayName);

            // Add the external login information to the user.
            var addLoginResult = await userManager.AddLoginAsync(user, externalLoginInfo);

            // Return the result of adding the external login to the user
            return addLoginResult;
        }

        /// <summary>
        /// Creates a role with the specified external login role.
        /// </summary>
        /// <param name="externalLoginRole">The external login role to be created.</param>
        /// <returns>An IdentityResult indicating the result of role creation.</returns>
        private async Task<IdentityResult> CreateRole(ApplicationRole externalLoginRole)
        {
            // Grant permissions for the external login role.
            await GrantPermissionsForExternalLoginRole(externalLoginRole);

            // Create the external login role using the RoleManager.
            var externalLoginRoleResult = await roleManager.CreateAsync(externalLoginRole);

            return externalLoginRoleResult;
        }

        /// <summary>
        /// Grants permissions for the external login role.
        /// </summary>
        /// <param name="externalLoginRole">The external login role.</param>
        private async Task GrantPermissionsForExternalLoginRole(ApplicationRole externalLoginRole)
        {
            // List of external login permissions.
            var externalLoginPermissionsList = new List<ApplicationPermission>
                                               {
                                                   new() {Name = "Applicants"},
                                                   new() {Name = "Applicants.CreateApplicant"},
                                                   new() {Name = "Applicants.DeleteApplicant"},
                                                   new() {Name = "Applicants.GetApplicant"},
                                                   new() {Name = "Applicants.GetApplicants"},
                                                   new() {Name = "Applicants.UpdateApplicant"},
                                                   new() {Name = "Applicants.ExportAsPdf"},
                                                   new() {Name = "Dashboard.GetHeadlinesData"}
                                               };

            // Get external login permissions from the DbContext.
            var externalLoginPermissions = (await dbContext.ApplicationPermissions.ToListAsync())
                    .Where(p => externalLoginPermissionsList.Any(fpop => fpop.Name == p.Name));

            // Add external login permissions to the external login role.
            foreach (var permission in externalLoginPermissions)
            {
                externalLoginRole.RoleClaims.Add(new ApplicationRoleClaim
                {
                    ClaimType = "permissions",
                    ClaimValue = permission.Name
                });
            }
        }

        #endregion Private Methods
    }

    #endregion Public Classes
}