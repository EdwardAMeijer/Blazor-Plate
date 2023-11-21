namespace BinaryPlate.BlazorPlate.Contracts.Services;

public interface IHttpService
{
    #region Public Methods

    /// <summary>
    /// Sends an HTTP GET request to the specified URL and returns the deserialized response.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response object.</typeparam>
    /// <param name="url">The URL to send the GET request to.</param>
    /// <param name="defaultClient">The default client to use for the request (optional).</param>
    /// <returns>An ApiResponseWrapper containing the deserialized response.</returns>
    Task<ApiResponseWrapper<TResponse>> Get<TResponse>(string url, string defaultClient = HttpClientType.DefaultClient);

    /// <summary>
    /// Sends an HTTP GET request to the specified URL with the provided data and returns the
    /// deserialized response.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request data.</typeparam>
    /// <typeparam name="TResponse">The type of the response object.</typeparam>
    /// <param name="url">The URL to send the GET request to.</param>
    /// <param name="data">The request data.</param>
    /// <param name="defaultClient">The default client to use for the request (optional).</param>
    /// <returns>An ApiResponseWrapper containing the deserialized response.</returns>
    Task<ApiResponseWrapper<TResponse>> Get<TRequest, TResponse>(string url, TRequest data, string defaultClient = HttpClientType.DefaultClient);

    /// <summary>
    /// Sends an HTTP POST request to the specified URL with the provided data and returns the
    /// deserialized response.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request data.</typeparam>
    /// <typeparam name="TResponse">The type of the response object.</typeparam>
    /// <param name="url">The URL to send the POST request to.</param>
    /// <param name="data">The request data.</param>
    /// <param name="defaultClient">The default client to use for the request (optional).</param>
    /// <returns>An ApiResponseWrapper containing the deserialized response.</returns>
    Task<ApiResponseWrapper<TResponse>> Post<TRequest, TResponse>(string url, TRequest data, string defaultClient = HttpClientType.DefaultClient);

    /// <summary>
    /// Sends an HTTP POST request to the specified URL and returns the deserialized response.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response object.</typeparam>
    /// <param name="url">The URL to send the POST request to.</param>
    /// <param name="defaultClient">The default client to use for the request (optional).</param>
    /// <returns>An ApiResponseWrapper containing the deserialized response.</returns>
    Task<ApiResponseWrapper<TResponse>> Post<TResponse>(string url, string defaultClient = HttpClientType.DefaultClient);

    /// <summary>
    /// Sends an HTTP POST request with multipart/form-data to the specified URL and returns the
    /// deserialized response.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request data.</typeparam>
    /// <typeparam name="TResponse">The type of the response object.</typeparam>
    /// <param name="url">The URL to send the POST request to.</param>
    /// <param name="data">The multipart/form-data content.</param>
    /// <param name="defaultClient">The default client to use for the request (optional).</param>
    /// <param name="cancellationToken">A cancellation token to cancel the request (optional).</param>
    /// <returns>An ApiResponseWrapper containing the deserialized response.</returns>
    Task<ApiResponseWrapper<TResponse>> PostFormData<TRequest, TResponse>(string url, MultipartFormDataContent data, string defaultClient = HttpClientType.DefaultClient, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends an HTTP PUT request to the specified URL with the provided data and returns the
    /// deserialized response.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request data.</typeparam>
    /// <typeparam name="TResponse">The type of the response object.</typeparam>
    /// <param name="url">The URL to send the PUT request to.</param>
    /// <param name="data">The request data.</param>
    /// <param name="defaultClient">The default client to use for the request (optional).</param>
    /// <returns>An ApiResponseWrapper containing the deserialized response.</returns>
    Task<ApiResponseWrapper<TResponse>> Put<TRequest, TResponse>(string url, TRequest data, string defaultClient = HttpClientType.DefaultClient);

    /// <summary>
    /// Sends an HTTP PUT request with multipart/form-data to the specified URL and returns the
    /// deserialized response.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request data.</typeparam>
    /// <typeparam name="TResponse">The type of the response object.</typeparam>
    /// <param name="url">The URL to send the PUT request to.</param>
    /// <param name="data">The multipart/form-data content.</param>
    /// <param name="defaultClient">The default client to use for the request (optional).</param>
    /// <param name="cancellationToken">A cancellation token to cancel the request (optional).</param>
    /// <returns>An ApiResponseWrapper containing the deserialized response.</returns>
    Task<ApiResponseWrapper<TResponse>> PutFormData<TRequest, TResponse>(string url, MultipartFormDataContent data, string defaultClient = HttpClientType.DefaultClient, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends an HTTP DELETE request to the specified URL and returns the deserialized response.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response object.</typeparam>
    /// <param name="url">The URL to send the DELETE request to.</param>
    /// <param name="defaultClient">The default client to use for the request (optional).</param>
    /// <returns>An ApiResponseWrapper containing the deserialized response.</returns>
    Task<ApiResponseWrapper<TResponse>> Delete<TResponse>(string url, string defaultClient = HttpClientType.DefaultClient);

    #endregion Public Methods
}