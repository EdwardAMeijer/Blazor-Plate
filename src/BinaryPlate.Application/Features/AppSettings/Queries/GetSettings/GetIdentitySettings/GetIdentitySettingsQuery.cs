namespace BinaryPlate.Application.Features.AppSettings.Queries.GetSettings.GetIdentitySettings;

public class GetIdentitySettingsQuery : IRequest<Envelope<GetIdentitySettingsForEditResponse>>
{
    #region Public Classes

    public class GetIdentitySettingsQueryHandler(IAppSettingsService appSettingsService) : IRequestHandler<GetIdentitySettingsQuery, Envelope<GetIdentitySettingsForEditResponse>>
    {
        #region Public Methods

        public async Task<Envelope<GetIdentitySettingsForEditResponse>> Handle(GetIdentitySettingsQuery request, CancellationToken cancellationToken)
        {
            return await appSettingsService.GetIdentitySettings();
        }

        #endregion Public Methods
    }

    #endregion Public Classes
}