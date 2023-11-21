namespace BinaryPlate.BlazorPlate.Providers;

public class UrlProvider(IConfiguration configuration) : IUrlProvider
{
    #region Public Properties

    public string BaseApiUrl => configuration["BaseApiUrl"];
    public string BaseHubApiUrl => configuration["BaseApiUrl"]?.Split("/api/")[0];
    public string BaseClientUrl => configuration["BaseClientUrl"];
    public string LoginRedirectRelativePath => "account/ExternalLogin";

    #endregion Public Properties
}