namespace BinaryPlate.WebAPI.Controllers;

[Route("api/[controller]")]
[BpAuthorize]
public class DashboardController(IHubContext<DashboardHub> dashboardHubContext, TimerManager timerManager) : ApiController
{
    #region Public Methods

    [HttpPost("GetHeadlinesData")]
    public async Task<IActionResult> GetHeadlinesData()
    {
        var response = await Sender.Send(new GetHeadlinesQuery());

        timerManager.PrepareTimer(() => dashboardHubContext.Clients.All.SendAsync("SendHeadlinesData", response.Payload));

        return TryGetResult(response);
    }

    #endregion Public Methods
}