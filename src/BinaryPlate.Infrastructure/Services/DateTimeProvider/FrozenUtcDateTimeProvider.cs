namespace BinaryPlate.Infrastructure.Services.DateTimeProvider;

/// <summary>
/// Provides a fixed and consistent value for the current UTC time that does not change throughout
/// the lifetime of the request which can improve consistency, determinism, and reliability of the
/// application, especially in situations where time-sensitive information is used or tested.
/// </summary>
public class FrozenUtcDateTimeProvider : IUtcDateTimeProvider
{
    #region Private Fields

    private readonly DateTime _utcTime;

    #endregion Private Fields

    #region Public Constructors

    public FrozenUtcDateTimeProvider(DateTime utcTime)
    {
        _utcTime = utcTime.Kind == DateTimeKind.Utc ? utcTime : throw new ArgumentException(Resource.The_time_provided_must_be_in_UTC_format, nameof(utcTime));
    }

    public FrozenUtcDateTimeProvider(IUtcDateTimeProvider utcDateTime)
    {
        _utcTime = utcDateTime.GetUtcNow();
    }

    #endregion Public Constructors

    #region Public Methods

    public DateTime GetUtcNow() => _utcTime;

    public long GetUnixTimeSeconds() => new DateTimeOffset(_utcTime).ToUnixTimeSeconds();

    public long GetUnixTimeMilliseconds() => new DateTimeOffset(_utcTime).ToUnixTimeMilliseconds();

    #endregion Public Methods
}