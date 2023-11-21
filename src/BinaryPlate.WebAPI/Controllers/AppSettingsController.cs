namespace BinaryPlate.WebAPI.Controllers;

[Route("api/[controller]")]
[BpAuthorize]
public class AppSettingsController : ApiController
{
    #region Public Methods

    [ProducesResponseType(typeof(ApiSuccessResponse<GetIdentitySettingsForEditResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpGet("GetIdentitySettings")]
    public async Task<IActionResult> GetIdentitySettings()
    {
        var response = await Sender.Send(new GetIdentitySettingsQuery());
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<GetIdentitySettingsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPut("UpdateIdentitySettings")]
    public async Task<IActionResult> UpdateIdentitySettings(UpdateIdentitySettingsCommand request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<GetFileStorageSettingsForEditResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpGet("GetFileStorageSettings")]
    public async Task<IActionResult> GetFileStorageSettings()
    {
        var response = await Sender.Send(new GetFileStorageSettingsQuery());
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<GetFileStorageSettingsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPut("UpdateFileStorageSettings")]
    public async Task<IActionResult> UpdateFileStorageSettings(UpdateFileStorageSettingsCommand request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<GetTokenSettingsForEditResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpGet("GetTokenSettings")]
    public async Task<IActionResult> GetTokenSettings()
    {
        var response = await Sender.Send(new GetTokenSettingsQuery());
        return TryGetResult(response);
    }

    [ProducesResponseType(typeof(ApiSuccessResponse<GetTokenSettingsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    [HttpPut("UpdateTokenSettings")]
    public async Task<IActionResult> UpdateTokenSettings(UpdateTokenSettingsCommand request)
    {
        var response = await Sender.Send(request);
        return TryGetResult(response);
    }

    #endregion Public Methods
}