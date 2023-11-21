namespace BinaryPlate.Infrastructure.Services;

public class AppSeederService : IAppSeederService
{
    #region Private Fields

    private readonly ApplicationUserManager _userManager;
    private readonly ApplicationRoleManager _roleManager;
    private readonly IApplicationDbContext _dbContext;
    private readonly IPermissionScanner _permissionScanner;
    private readonly IOptions<IdentityOptions> _identityOptions;

    #endregion Private Fields

    #region Public Constructors

    public AppSeederService(ApplicationUserManager userManager,
                            ApplicationRoleManager roleManager,
                            IApplicationDbContext dbContext,
                            IPermissionScanner permissionScanner,
                            IOptions<IdentityOptions> identityOptions)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _dbContext = dbContext;
        _identityOptions = identityOptions;
        _permissionScanner = permissionScanner;
        DisablePasswordComplexity();
    }

    #endregion Public Constructors

    #region Public Methods

    public async Task<bool> SeedApp()
    {
        // Scan and seed the built-in permissions.
        await _permissionScanner.ScanAndSeedBuiltInPermissions();

        // Create a new admin user.
        var adminUser = new ApplicationUser
        {
            Email = "admin@demo",
            UserName = "admin@demo",
            Name = "Marcella",
            Surname = "Wallace",
            JobTitle = "Administrator",
            EmailConfirmed = true,
            IsStatic = true,
            IsSuperAdmin = true
        };

        // Create a new admin role.
        var adminRole = new ApplicationRole
        {
            Name = "Admin",
            IsStatic = true
        };

        // Grant permissions to the admin role for single-tenant applications.
        await GrantPermissionsAdminRole(adminRole);

        // Create the admin role using the RoleManager.
        var adminRoleResult = await _roleManager.CreateAsync(adminRole);

        if (!adminRoleResult.Succeeded)
            // Return the admin role result if it was not successful.
            return false;

        // Create the admin user using the UserManager.
        var adminUserResult = await _userManager.CreateAsync(adminUser, "123456");

        if (!adminUserResult.Succeeded)
            // Return the admin user result if it was not successful.
            return false;

        // Add the admin user to the admin role using the UserManager.
        var adminAddToRoleResult = await _userManager.AddToRoleAsync(adminUser, adminRole.Name);

        return adminAddToRoleResult.Succeeded;
    }

    #endregion Public Methods

    #region Private Methods

    /// <summary>
    /// Grants permissions to the admin role.
    /// </summary>
    /// <param name="adminRole">The admin role to grant permissions to.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private async Task GrantPermissionsAdminRole(ApplicationRole adminRole)
    {
        // Retrieve permissions.
        var permissions = await _dbContext.ApplicationPermissions.IgnoreQueryFilters().ToListAsync();

        // Iterate through each permission and add it to the admin role if it does not exist.
        foreach (var permission in permissions)
        {
            // Check if the permission already exists in the admin role.
            var isClaimExist = adminRole.RoleClaims.Any(rc => rc.ClaimValue == permission.Name);

            // If the permission does not exist, add it to the admin role.
            if (!isClaimExist)
                adminRole.RoleClaims.Add(new ApplicationRoleClaim
                {
                    ClaimType = "permissions",
                    ClaimValue = permission.Name
                });
        }
    }

    /// <summary>
    /// Disables password complexity requirements.
    /// </summary>
    private void DisablePasswordComplexity()
    {
        // Disable requiring a digit in the password.
        _identityOptions.Value.Password.RequireDigit = false;

        // Disable requiring a lowercase letter in the password.
        _identityOptions.Value.Password.RequireLowercase = false;

        // Disable requiring a non-alphanumeric character in the password.
        _identityOptions.Value.Password.RequireNonAlphanumeric = false;

        // Disable requiring an uppercase letter in the password.
        _identityOptions.Value.Password.RequireUppercase = false;
    }

    #endregion Private Methods
}