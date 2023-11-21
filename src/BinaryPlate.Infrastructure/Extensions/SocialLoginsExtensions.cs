namespace BinaryPlate.Infrastructure.Extensions;

/// <summary>
/// Extensions for configuring custom authentication providers.
/// </summary>
public static class SocialLoginsExtensions
{
    #region Public Methods

    /// <summary>
    /// Configures Twitter authentication.
    /// </summary>
    /// <returns>The <see cref="AuthenticationBuilder"/> instance.</returns>
    /// <remarks>
    /// [Obtaining Client ID and secret from Twitter Developer Portal]
    /// 1. Go to the Twitter Developer Portal (https://developer.twitter.com).
    /// 2. Sign in with your Twitter account or create a new account if needed.
    /// 3. Navigate to the "Apps" section or create a new app.
    /// 4. Fill in the required information for your app.
    /// 5. Obtain the Consumer Key (API Key) and Consumer Secret (API Secret Key) from the app settings.
    /// </remarks>
    public static AuthenticationBuilder AddTwitter(this AuthenticationBuilder builder, IConfiguration configuration)
    {
        builder.AddTwitter(twitterOptions =>
                           {
                               twitterOptions.ConsumerKey = configuration["Authentication:Twitter:ConsumerKey"] ?? string.Empty;
                               twitterOptions.ConsumerSecret = configuration["Authentication:Twitter:ConsumerSecret"] ?? string.Empty;
                           });

        return builder;
    }

    /// <summary>
    /// Configures Google authentication.
    /// </summary>
    /// <returns>The <see cref="AuthenticationBuilder"/> instance.</returns>
    /// <remarks>
    /// [Obtaining Client ID and secret from Google Console]
    /// 1. Go to the Google API Console (https://console.cloud.google.com).
    /// 2. Create a new project or select an existing project.
    /// 3. Enable the necessary APIs for authentication, such as "Google Sign-In API" or "Google
    /// Identity Platform".
    /// 4. Go to "Credentials" in the left menu.
    /// 5. Create credentials and select the appropriate credential type (e.g., OAuth client ID).
    /// 6. Obtain the Client ID and Client Secret from the credential settings.
    /// </remarks>
    public static AuthenticationBuilder AddGoogle(this AuthenticationBuilder builder, IConfiguration configuration)
    {
        builder.AddGoogle(googleOptions =>
                          {
                              googleOptions.ClientId = configuration["Authentication:Google:ClientId"] ?? string.Empty;
                              googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"] ?? string.Empty;
                          });

        return builder;
    }

    /// <summary>
    /// Configures LinkedIn authentication.
    /// </summary>
    /// <returns>The <see cref="AuthenticationBuilder"/> instance.</returns>
    /// <remarks>
    /// [Obtaining Client ID and secret from LinkedIn Developer Portal]
    /// 1. Go to the LinkedIn Developer Portal (https://developer.linkedin.com).
    /// 2. Sign in with your LinkedIn account or create a new account if needed.
    /// 3. Navigate to "My Apps" or create a new app.
    /// 4. Fill in the required information for your app.
    /// 5. Obtain the Client ID and Client Secret from the app settings.
    /// </remarks>
    public static AuthenticationBuilder AddLinkedIn(this AuthenticationBuilder builder, IConfiguration configuration)
    {
        builder.AddLinkedIn(linkedInOptions =>
                            {
                                linkedInOptions.ClientId = configuration["Authentication:LinkedIn:ClientId"] ?? string.Empty;
                                linkedInOptions.ClientSecret = configuration["Authentication:LinkedIn:ClientSecret"] ?? string.Empty;
                            });

        return builder;
    }

    /// <summary>
    /// Configures Microsoft authentication.
    /// </summary>
    /// <returns>The <see cref="AuthenticationBuilder"/> instance.</returns>
    /// <remarks>
    /// [Obtaining Client ID and secret from Microsoft Azure]
    /// 1. Go to the Azure Portal (https://portal.azure.com).
    /// 2. Navigate to your Azure Active Directory (AAD) tenant.
    /// 3. Select "App registrations" or "Azure Active Directory" &gt; "App registrations" from the
    /// left menu.
    /// 4. Create a new app registration if needed or select an existing one.
    /// 5. Obtain the Client ID from the app registration settings.
    /// 6. Generate or obtain the Client Secret (also known as the application key) from the app
    /// registration settings.
    /// </remarks>
    public static AuthenticationBuilder AddMicrosoft(this AuthenticationBuilder builder, IConfiguration configuration)
    {
        builder.AddMicrosoftAccount(microsoftOptions =>
                                    {
                                        microsoftOptions.ClientId = configuration["Authentication:Microsoft:ClientId"] ?? string.Empty;
                                        microsoftOptions.ClientSecret = configuration["Authentication:Microsoft:ClientValue"] ?? string.Empty;
                                        microsoftOptions.AuthorizationEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/authorize";
                                        microsoftOptions.TokenEndpoint = "https://login.microsoftonline.com/consumers/oauth2/v2.0/token";
                                    });

        return builder;
    }

    #endregion Public Methods

}