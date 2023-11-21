using BinaryPlate.BlazorPlate.Services.Handlers;

namespace BinaryPlate.BlazorPlate.Extensions;

public static class HttpInterceptorExtensions
{
    #region Public Methods

    public static void AddHttpInterceptor(this IServiceCollection services, WebAssemblyHostBuilder builder)
    {
        services.AddScoped<HttpInterceptor>();
        services.AddHttpClient(HttpClientType.DefaultClient, client =>
        {
            client.BaseAddress = new Uri(builder.Configuration.GetSection("BaseApiUrl").Value ?? throw new InvalidOperationException("Invalid BaseApiUrl."));
        }).AddHttpMessageHandler<HttpInterceptor>();
    }

    public static void AddHttpExternalAuth(this IServiceCollection services, WebAssemblyHostBuilder builder)
    {
        services.AddScoped<CookieHandler>();
        services.AddHttpClient(HttpClientType.ExternalAuthClient, client =>
        {
            client.BaseAddress = new Uri(builder.Configuration.GetSection("BaseApiUrl").Value ?? throw new InvalidOperationException("Invalid BaseApiUrl."));
        }).AddHttpMessageHandler<CookieHandler>();
    }

    #endregion Public Methods
}