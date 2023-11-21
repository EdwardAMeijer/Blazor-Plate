namespace BinaryPlate.BlazorPlate.Models;

public class ApiResponseWrapper<T>(T payload, bool isSuccessStatusCode, HttpStatusCode httpStatusCode)
{
    #region Public Constructors

    public ApiResponseWrapper(ApiErrorResponse apiErrorResponse, bool isSuccessStatusCode, HttpStatusCode httpStatusCode) : this(default(T), isSuccessStatusCode, httpStatusCode)
    {
        ApiErrorResponse = apiErrorResponse;
    }

    #endregion Public Constructors

    #region Public Properties

    public bool IsSuccessStatusCode { get; } = isSuccessStatusCode;
    public HttpStatusCode HttpStatusCode { get; set; } = httpStatusCode;
    public T Payload { get; } = payload;
    public ApiErrorResponse ApiErrorResponse { get; }

    #endregion Public Properties
}