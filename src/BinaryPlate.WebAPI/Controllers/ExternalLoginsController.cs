namespace BinaryPlate.WebAPI.Controllers;

[Route("api/account/[controller]")]
public class ExternalLoginsController : ApiController
{
    #region Public Methods

    [ProducesResponseType(typeof(ApiSuccessResponse<AuthenticateExternalLoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpGet("microsoft-account-login")]
    public IActionResult MicrosoftAccountLogin(string returnUrl)
    {
        Url.Action(nameof(MicrosoftAccountLoginCallBack), new { returnURL = returnUrl });

        return Challenge(new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(MicrosoftAccountLoginCallBack), new { returnURL = returnUrl })
        }, MicrosoftAccountDefaults.AuthenticationScheme);
    }

    [HttpGet("microsoft-account-login-callback")]
    public async Task<IActionResult> MicrosoftAccountLoginCallBack(string returnUrl)
    {
        var result = await Sender.Send(new AuthenticateExternalLoginCommand(MicrosoftAccountDefaults.AuthenticationScheme));

        if (!result.IsError)
        {
            await RefreshExternalSignIn(result.Payload.Claims);
            return Redirect($"{returnUrl}");
        }

        return TryGetResult(result);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<AuthenticateExternalLoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpGet("google-account-login")]
    public IActionResult GoogleAccountLogin(string returnUrl)
    {
        Url.Action(nameof(GoogleAccountLoginCallBack), new { returnURL = returnUrl });

        return Challenge(new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(GoogleAccountLoginCallBack), new { returnURL = returnUrl })
        }, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google-account-login-callback")]
    public async Task<IActionResult> GoogleAccountLoginCallBack(string returnUrl)
    {
        var result = await Sender.Send(new AuthenticateExternalLoginCommand(GoogleDefaults.AuthenticationScheme));

        if (!result.IsError)
        {
            await RefreshExternalSignIn(result.Payload.Claims);
            return Redirect($"{returnUrl}");
        }

        return TryGetResult(result);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<AuthenticateExternalLoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpGet("twitter-account-login")]
    public IActionResult TwitterAccountLogin(string returnUrl)
    {
        Url.Action(nameof(TwitterAccountLoginCallBack), new { returnURL = returnUrl });
        // Encode the returnUrl as a state parameter
        return Challenge(new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(TwitterAccountLoginCallBack), new { returnURL = returnUrl })
        }, TwitterDefaults.AuthenticationScheme);
    }

    [HttpGet("twitter-account-login-callback")]
    public async Task<IActionResult> TwitterAccountLoginCallBack(string returnUrl)
    {
        var result = await Sender.Send(new AuthenticateExternalLoginCommand(TwitterDefaults.AuthenticationScheme));

        if (!result.IsError)
        {
            await RefreshExternalSignIn(result.Payload.Claims);
            return Redirect($"{returnUrl}");
        }

        return TryGetResult(result);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<AuthenticateExternalLoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpGet("linkedIn-account-login")]
    public IActionResult LinkedInAccountLogin(string returnUrl)
    {
        Url.Action(nameof(LinkedInAccountLoginCallBack), new { returnURL = returnUrl });

        return Challenge(new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(LinkedInAccountLoginCallBack), new { returnURL = returnUrl }),
        }, LinkedInAuthenticationDefaults.AuthenticationScheme);
    }

    [HttpGet("linkedIn-account-login-callback")]
    public async Task<IActionResult> LinkedInAccountLoginCallBack(string returnUrl)
    {
        var result = await Sender.Send(new AuthenticateExternalLoginCommand(LinkedInAuthenticationDefaults.AuthenticationScheme));

        if (!result.IsError)
        {
            await RefreshExternalSignIn(result.Payload.Claims);
            return Redirect($"{returnUrl}");
        }

        return TryGetResult(result);
    }

    [HttpGet("RegisterExternalLogin")]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public async Task<IActionResult> RegisterExternalLogin()
    {
        var user = HttpContext.User;

        if (user.IsAuthenticated())
        {
            var userInfo = new UserInfo
            {
                Email = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                FirstName = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value,
                LastName = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value,
                DisplayName = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
                LoginProvider = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.AuthenticationMethod)?.Value,
                ProviderKey = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
            };

            var response = await Sender.Send(new RegisterExternalLoginCommand(userInfo));

            return TryGetResult(response);
        }
        return TryGetResult(Envelope<RegisterExternalLoginResponse>.Result.Unauthorized(Resource.User_is_not_found));
    }

    #endregion Public Methods

    #region Private Methods

    private async Task RefreshExternalSignIn(IEnumerable<Claim> claims)
    {
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties();

        HttpContext.User.AddIdentity(claimsIdentity);

        await HttpContext.SignOutAsync();

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                      new ClaimsPrincipal(claimsIdentity),
                                      authProperties);
    }

    #endregion Private Methods
}