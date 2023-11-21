namespace BinaryPlate.Application.Common.Contracts.WebAPI.Hub;

/// <summary>
/// Manages SignalR application context.
/// </summary>
public interface ISignalRContextProvider
{
    #region Public Methods

    /// <summary>
    /// Gets the host name of the SignalR connection.
    /// </summary>
    /// <param name="hubCallerContext">The context of the SignalR connection.</param>
    /// <returns>The host name of the SignalR connection.</returns>
    string GetHostName(HubCallerContext hubCallerContext);

    /// <summary>
    /// Gets the user identifier of the user associated with the SignalR connection.
    /// </summary>
    /// <param name="hubCallerContext">The context of the SignalR connection.</param>
    /// <returns>The user identifier of the user associated with the SignalR connection.</returns>
    string GetUserNameIdentifier(HubCallerContext hubCallerContext);

    /// <summary>
    /// Gets the name of the user associated with the SignalR connection.
    /// </summary>
    /// <param name="hubCallerContext">The context of the SignalR connection.</param>
    /// <returns>The name of the user associated with the SignalR connection.</returns>
    string GetUserName(HubCallerContext hubCallerContext);

    #endregion Public Methods
}