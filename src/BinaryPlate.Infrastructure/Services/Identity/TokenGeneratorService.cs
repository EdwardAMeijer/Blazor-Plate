namespace BinaryPlate.Infrastructure.Services.Identity;

public class TokenGeneratorService(UserManager<ApplicationUser> userManager,
                                   IUtcDateTimeProvider utcDateTimeProvider,
                                   IPermissionService permissionService,
                                   IAppOptionsService appOptionsService,
                                   IAppSettingsService appSettingsService) : ITokenGeneratorService
{
    #region Public Methods

    public async Task<string> GenerateAccessTokenAsync(ApplicationUser user, bool hasPassword = true)
    {
        // Builds user claims based on user information.
        var claims = await BuildUserClaims(user, hasPassword);

        // Builds a token using the user claims.
        var token = await BuildToken(claims);

        // Writes the token as a JWT token and returns it as a string.
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Method to generate a refresh token.
    public string GenerateRefreshToken()
    {
        // Generates a 32-byte random number and converts it to a Base64 string.
        var randomNumber = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    // Method to get principal from an expired token.
    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        // Throws an exception if the token is null or empty.
        if (string.IsNullOrWhiteSpace(token))
            throw new SecurityTokenException(Resource.Invalid_token);

        // Defines token validation parameters.
        var tokenValidationParameters = new TokenValidationParameters
        {
            // Validate that the audience of the token matches the intended audience (recipient) of
            // the token.
            ValidateAudience = true,
            // Validate that the issuer of the token is a trusted issuer.
            ValidateIssuer = true,
            // Validate that the security key used to sign the token is valid and trusted.
            ValidateIssuerSigningKey = true,
            // The security key used to sign the token.
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appOptionsService.GetAppJwtOptions().SecurityKey)),
            // Do not validate the lifetime of the token, since we are using a custom expiration utcDateTime.
            ValidateLifetime = false,
            // The valid issuer of the token.
            ValidIssuer = appOptionsService.GetAppJwtOptions().Issuer,
            // The intended audience (recipient) of the token.
            ValidAudience = appOptionsService.GetAppJwtOptions().Audience,
            // Optional: specify a different audience (recipient) of the token.
            //ValidAudience = _appOptionsService.GetAppJwtOptions().Audience,
        };

        // Validates the token and retrieves the principal and security token.
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        // Casts the security token as a JWT security token.
        var jwtSecurityToken = securityToken as JwtSecurityToken;

        // Throws an exception if the algorithm used to sign the token is invalid.
        if (jwtSecurityToken?.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase) != true)
            throw new SecurityTokenException(Resource.Invalid_token);

        // Returns the principal.
        return principal;
    }

    #endregion Public Methods

    #region Private Methods

    private async Task<JwtSecurityToken> BuildToken(IEnumerable<Claim> claims)
    {
        // Creates a SymmetricSecurityKey using the provided JWT security key.
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appOptionsService.GetAppJwtOptions().SecurityKey));

        // Creates SigningCredentials using the symmetric security key and HmacSha256 algorithm.
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        // Retrieves token settings from the application settings use case.
        var tokenSettingsResponse = await appSettingsService.GetTokenSettings();

        // Extracts the access token utcDateTime span from the token settings response.
        var accessTokenTimeSpan = tokenSettingsResponse.Payload.AccessTokenTimeSpan;

        // Throws an exception if the access token utcDateTime span is null.
        if (accessTokenTimeSpan == null)
            throw new Exception(Resource.Access_token_timespan_cannot_be_null);

        // Calculates the expiry utcDateTime of the token based on the current UTC utcDateTime and the
        // access token utcDateTime span.
        var expiry = utcDateTimeProvider.GetUtcNow().AddMinutes(accessTokenTimeSpan.Value);

        // Creates a new JWT security token using the provided issuer, audience, claims, expiry
        // utcDateTime, and signing credentials.
        var token = new JwtSecurityToken(issuer: appOptionsService.GetAppJwtOptions().Issuer,
                                         audience: appOptionsService.GetAppJwtOptions().Audience,
                                         claims: claims,
                                         expires: expiry,
                                         signingCredentials: signingCredentials);

        // Returns the JWT security token.
        return token;
    }

    private async Task<List<Claim>> BuildUserClaims(ApplicationUser user, bool hasPassword = true)
    {
        // Create a new list of claims for the user.
        var claims = new List<Claim>
                 {
                     // Add a unique identifier for the token.
                     new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                     // Add the user's ID as the subject of the token.
                     new(JwtRegisteredClaimNames.Sub, user.Id),
                     // Add the user's ID as the name identifier.
                     new(ClaimTypes.NameIdentifier, user.Id),
                     // Add the user's username or an empty string as the name.
                     new(ClaimTypes.Name, user.UserName ?? string.Empty),
                     // Add the user's email or an empty string as the email.
                     new(ClaimTypes.Email, user.Email ?? string.Empty),
                     // Indicate whether the user's account has a password associated with it.
                     new("HasPassword", hasPassword.ToString().ToLower()),
                     // Add a claim indicating whether the user is a super admin.
                     new("IsSuperAdmin", user.IsSuperAdmin.ToString().ToLower()),
                     // Add the user's avatar URI or an empty string as the avatar URI.
                     new("AvatarUri", user.AvatarUri ?? string.Empty),
                     // Add the user's full name or their username as the full name.
                     new("FullName", (string.IsNullOrWhiteSpace(user.FullName) ? user.UserName : user.FullName) ?? string.Empty),
                     // Add the user's job title or an empty string as the job title.
                     new("JobTitle", user.JobTitle ?? string.Empty),
                     // Add a claim indicating when the refresh token will expire.
                     new("refreshAt", ((DateTimeOffset) user.RefreshTokenTimeSpan).ToUnixTimeSeconds().ToString()),
                 };

        // Add the user's roles as claims.
        await GetUserRolesAsClaims(user, claims);

        // Add the user's permissions as claims.
        await GetUserPermissionsAsClaims(user, claims);

        return claims;
    }

    private async Task GetUserPermissionsAsClaims(ApplicationUser user, List<Claim> claims)
    {
        // Get non-excluded permissions for the user from the use case layer.
        var userNonExcludedPermissions = await permissionService.GetUserPermissionsExceptExcluded(user);

        // Create a list of claims from the userNonExcludedPermissions.
        var userPermissionsAsClaims = userNonExcludedPermissions.Select(nep => new Claim("permissions", nep.Name));

        // Add all permission claims to the existing claims list.
        claims.AddRange(userPermissionsAsClaims);
    }

    private async Task GetUserRolesAsClaims(ApplicationUser user, List<Claim> claims)
    {
        // Get all roles for the user using the user manager.
        var userRoles = await userManager.GetRolesAsync(user);

        // Create a list of claims from the userRoles.
        var userRolesAsClaims = userRoles.Select(r => new Claim(ClaimTypes.Role, r));

        // Add all role claims to the existing claims list.
        claims.AddRange(userRolesAsClaims);
    }

    #endregion Private Methods
}