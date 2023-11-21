namespace BinaryPlate.WebAPI.Controllers;

[Route("api/[controller]")]
[BpAuthorize]
public class PermissionsController : ApiController
{
    #region Public Methods

    [ProducesResponseType(typeof(ApiSuccessResponse<GetPermissionsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPost("GetPermissions")]
    public async Task<IActionResult> GetPermissions(GetPermissionsQuery request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    #endregion Public Methods
}