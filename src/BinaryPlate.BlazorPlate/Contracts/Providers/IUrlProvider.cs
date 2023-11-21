namespace BinaryPlate.BlazorPlate.Contracts.Providers;

public interface IUrlProvider
{
    #region Public Properties

    string BaseApiUrl { get; }
    string BaseHubApiUrl { get; }
    string BaseClientUrl { get; }
    string LoginRedirectRelativePath { get; }

    #endregion Public Properties
}