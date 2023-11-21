namespace BinaryPlate.Infrastructure.Services.DateTimeProvider;

public class UtcDateTimeProvider : IUtcDateTimeProvider
{
    #region Public Methods

    public DateTime GetUtcNow() => TimeProvider.System.GetUtcNow().UtcDateTime;

    public long GetUnixTimeSeconds() => TimeProvider.System.GetUtcNow().ToUnixTimeSeconds();

    public long GetUnixTimeMilliseconds() => TimeProvider.System.GetUtcNow().ToUnixTimeMilliseconds();

    #endregion Public Methods
}